using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{
    public GameObject blink;             // 사진 찍을 때 깜빡일 것
    public GameObject shareButtons;      // 공유 버튼

    bool isCoroutinePlaying;             // 코루틴 중복방지

    // 파일 불러올 때 필요
    string albumName = "MyAlbum";           // 생성될 앨범의 이름
    [SerializeField]
    GameObject panel;                    // 찍은 사진이 뜰 패널


    // 캡쳐 버튼을 누르면 호출
    public void Capture_Button()
    {
        // 중복방지 bool
        if (!isCoroutinePlaying)
        {
            StartCoroutine("captureScreenshot");
        }
    }

    IEnumerator captureScreenshot()
    {
        isCoroutinePlaying = true;

        // UI 없앤다...

        yield return new WaitForEndOfFrame();

        // 스크린샷 + 갤러리갱신
        ScreenshotAndGallery();

        yield return new WaitForEndOfFrame();

        // 블링크
        BlinkUI();

        // 셔터 사운드 넣기...
        
        yield return new WaitForEndOfFrame();
        blink.SetActive(false);
        // UI 다시 나온다...

        yield return new WaitForSecondsRealtime(0.3f);

        // 찍은 사진이 등장
        //GetPirctureAndShowIt();

        isCoroutinePlaying = false;
    }

    // 흰색 블링크 생성
    void BlinkUI()
    {
        blink.SetActive(true);
        //GameObject b = Instantiate(blink);
        //b.transform.SetParent(transform);
        //b.transform.localPosition = new Vector3(0, 0, 0);
        //b.transform.localScale = new Vector3(1, 1, 1);
    }

    // 스크린샷 찍고 갤러리에 갱신
    void ScreenshotAndGallery()
    {
        // 스크린샷
        // 2d 텍스쳐객체 > 넓이, 높이, 포멧 RGB24 설정, true?false?
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        // 현재 화면을 픽셀단위로 읽음
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        // 적용
        ss.Apply();

        // 갤러리갱신
        Debug.Log("" + NativeGallery.SaveImageToGallery(ss, albumName,
            "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png"));

        // To avoid memory leaks.
        // 복사 완료됐기 때문에 원본 메모리 삭제
        Destroy(ss);

    }
    // 찍은 사진을 Panel에 보여준다.
    void GetPirctureAndShowIt()
    {
        string pathToFile = GetPicture.GetLastPicturePath();
        if (pathToFile == null)
        {
            return;
        }
        Texture2D texture = GetScreenshotImage(pathToFile);
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        panel.SetActive(true);
        shareButtons.SetActive(true);
        panel.GetComponent<Image>().sprite = sp;
    }
    // 찍은 사진을 불러온다.
    Texture2D GetScreenshotImage(string filePath)
    {
        Texture2D texture = null;
        byte[] fileBytes;
        if (File.Exists(filePath))
        {
            fileBytes = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(fileBytes);
        }
        return texture;
    }
}