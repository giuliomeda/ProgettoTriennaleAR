using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class importController : MonoBehaviour
{
    [SerializeField]
    private Button ChooseAPictureButton;

    [SerializeField]
    private Button TakePhotoButton;

    [SerializeField]
    private GameObject InstructionPanel;

    [SerializeField]
    private RawImage importedImage;

    private int rawImageOriginWidth;
    private int rawImageOriginHeight;

    [SerializeField]
    private Button backToMenuButton;


    private void Start() {
        RectTransform rt = importedImage.GetComponent<RectTransform>();
        // get the rect size as base size for the upcomming video
        rawImageOriginWidth = Mathf.RoundToInt(rt.rect.width);
        rawImageOriginHeight = Mathf.RoundToInt(rt.rect.height);
    }
    private void Awake() {
        ChooseAPictureButton.onClick.AddListener(choosePicture);
        TakePhotoButton.onClick.AddListener(takeAPhoto);
        backToMenuButton.onClick.AddListener(backToMenu);
    }

    private void backToMenu(){
        SceneManager.LoadScene("MainMenù");
    }

    /*private IEnumerator Wait()        //mi serviva per evitare che si vedesse lo sfondo bianco prima che l'immagine venisse importata
    {
        yield return new WaitForSeconds(2f);
        importedImage.gameObject.SetActive(true);

    }*/
    private void choosePicture(){
        InstructionPanel.gameObject.SetActive(false);
        PickImage(-1);
        //StartCoroutine(Wait());
        importedImage.gameObject.SetActive(true);
    }

    private void takeAPhoto(){
        InstructionPanel.gameObject.SetActive(false);
        TakePicture(-1);
        importedImage.gameObject.SetActive(true);

    }
    private void PickImage( int maxSize )
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                NativeGallery.ImageProperties properties = new NativeGallery.ImageProperties();
                properties = NativeGallery.GetImageProperties(path);
                // Create Texture from selected image
                Texture2D texture = new Texture2D (properties.width,properties.height);
                texture = NativeGallery.LoadImageAtPath( path, maxSize, false );

                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }
                
                int[] scaledTexture;
                scaledTexture = scaleResolution(texture.width,texture.height,rawImageOriginWidth,rawImageOriginHeight);
                importedImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaledTexture[0]);
                importedImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaledTexture[1]);
                importedImage.texture = texture;

            }
        });

        Debug.Log( "Permission result: " + permission );
    }
    private void TakePicture( int maxSize )
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }

                int[] scaledTexture;
                scaledTexture = scaleResolution(texture.width,texture.height,rawImageOriginWidth,rawImageOriginHeight);
                importedImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaledTexture[0]);
                importedImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaledTexture[1]);
                importedImage.texture = texture;
                
            }
        }, maxSize );

        Debug.Log( "Permission result: " + permission );
    }
    int[] scaleResolution(int width, int heigth, int maxWidth, int maxHeight)
    {
        int new_width = width;
        int new_height = heigth;

        if (width > heigth)
        {
            new_width = maxWidth;
            new_height = (new_width * heigth) / width;
        }
        else
        {
            new_height = maxHeight;
            new_width = (new_height * width) / heigth;
        }

        int[] dimension = { new_width, new_height };
        return dimension;
    }
}
