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
    private GameObject InstructionPanel;

    [SerializeField]
    private RawImage importedImage;

    [SerializeField]
    private Button backToMenuButton;

    private void Awake() {
        ChooseAPictureButton.onClick.AddListener(choosePicture);
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
    private void PickImage( int maxSize )
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath( path, maxSize );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }
                importedImage.texture = texture;
            }
        });

        Debug.Log( "Permission result: " + permission );
    }
}
