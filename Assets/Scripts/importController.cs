using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class importController : MonoBehaviour
{
    public static bool TextureIsSet = false;

    [SerializeField]
    private Button ChooseAPictureButton;

    [SerializeField]
    private Button TakePhotoButton;

    [SerializeField]
    private GameObject InstructionPanel;

    [SerializeField]
    private Button backToMenuButton;

    [SerializeField]
    private GameObject imageGameObject;


    private void Awake() {
        ChooseAPictureButton.onClick.AddListener(choosePicture);
        TakePhotoButton.onClick.AddListener(takeAPhoto);
        backToMenuButton.onClick.AddListener(backToMenu);
    }

    private void backToMenu(){
        TextureIsSet = false;
        SceneManager.LoadScene("MainMenù");
    }
    private void choosePicture(){
        InstructionPanel.gameObject.SetActive(false);
        PickImage(-1);
        //StartCoroutine(Wait());
        imageGameObject.gameObject.SetActive(true);
    }

    private void takeAPhoto(){
        InstructionPanel.gameObject.SetActive(false);
        TakePicture(-1);
        imageGameObject.gameObject.SetActive(true);

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
                
			    imageGameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
			    imageGameObject.transform.forward = Camera.main.transform.forward;
			    imageGameObject.transform.localScale = new Vector3( 1f, texture.height / (float) texture.width, 1f );

			    Material material = imageGameObject.GetComponent<Renderer>().material;
			    if( !material.shader.isSupported ) // happens when Standard shader is not included in the build
				    material.shader = Shader.Find( "Legacy Shaders/Diffuse" );

			    material.mainTexture = texture;
                TextureIsSet = true;

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
                
			    imageGameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
			    imageGameObject.transform.forward = Camera.main.transform.forward;
			    imageGameObject.transform.localScale = new Vector3( 1f, texture.height / (float) texture.width, 1f );

			    Material material = imageGameObject.GetComponent<Renderer>().material;
			    if( !material.shader.isSupported ) // happens when Standard shader is not included in the build
				    material.shader = Shader.Find( "Legacy Shaders/Diffuse" );

			    material.mainTexture = texture;
                TextureIsSet = true;

                
            }
        }, maxSize );

        Debug.Log( "Permission result: " + permission );
    }
    
}
