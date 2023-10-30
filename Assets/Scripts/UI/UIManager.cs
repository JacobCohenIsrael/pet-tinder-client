using System;
using Gamefather.Pet;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;

namespace Gamefather.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button likeButton;
        [SerializeField] private Button dislikeButton;
        [SerializeField] private Image petImage;
        private Pet.Pet[] pets;

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
            RestClient.Get<GetPetsListResponse>("http://localhost:3000/pet/list?petType=dog&petGender=male%2Cfemale").Then(
                response =>
                {
                    pets = response.pets;
                }    
            ).Catch(Debug.LogError);

        }

        private void OnLikeButtonClicked()
        {
            Debug.Log("Like");
        }
        
        private void OnDislikeButtonClicked()
        {
            Debug.Log("Dislike");
        }

    }
}
