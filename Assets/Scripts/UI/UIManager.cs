using System;
using System.Collections;
using Gamefather.Pet;
using Gamefather.Services;
using Proyecto26;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Gamefather.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button likeButton;
        [SerializeField] private Button dislikeButton;
        [SerializeField] private Image petImage;
        
        [SerializeField] private HttpService httpService;
        
        private Pet.Pet[] pets;
        private int petIndex;

        private void Awake()
        {
            likeButton.onClick.AddListener(OnLikeButtonClicked);
            dislikeButton.onClick.AddListener(OnDislikeButtonClicked);
        }

        private void OnDestroy()
        {
            likeButton.onClick.RemoveListener(OnLikeButtonClicked);
            dislikeButton.onClick.RemoveListener(OnDislikeButtonClicked);
        }

        private void Start()
        {
            httpService.Get<GetPetsListResponse>("pet/list/?petType=dog&petGender=male%2Cfemale").Then(
                response =>
                {
                    pets = response.pets;
                }    
            ).Then(LoadImage).Catch(Debug.LogError);

        }

        private void LoadImage()
        {
            var imageUrl = pets[petIndex].photos[0].large;
            StartCoroutine(LoadImageAsync(imageUrl));
        }

        private IEnumerator LoadImageAsync(string imageUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
                yield return null;
            }

            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            var downloadedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));

            petImage.sprite = downloadedSprite;
        }

        private void OnLikeButtonClicked()
        {
            httpService.Post<PetVoteRequest, object>(new PetVoteRequest { petId = pets[petIndex].id, like = true }, "pet/vote").Then(
                (response) =>
                {
                    ShowNextPet();
                }).Catch(Debug.LogError);
        }
        
        private void OnDislikeButtonClicked()
        {
            httpService.Post<PetVoteRequest, object>(new PetVoteRequest { petId = pets[petIndex].id, like = false }, "pet/vote").Then(
                (response) =>
                {
                    ShowNextPet();
                }).Catch(Debug.LogError);
        }

        private void ShowNextPet()
        {
            petIndex++;
            LoadImage();
        }
    }
}
