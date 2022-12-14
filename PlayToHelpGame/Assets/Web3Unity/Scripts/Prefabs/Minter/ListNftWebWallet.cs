using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Web3Unity.Scripts.Prefabs.Minter
{
    public class ListNftWebWallet : MonoBehaviour
    {
        private string chain = "ethereum";
        private string network = "goerli";
        private string chainID = "5";
        private string _itemPrice = "0.001";
        private string _tokenType = "";
        private string _itemID = "";
        private string account;
        
        public Image textureObject;
        public TextMeshProUGUI description;
        public TextMeshProUGUI tokenURI;
        public TextMeshProUGUI contractAddr;
        public TextMeshProUGUI isApproved;
        public TMP_InputField itemPrice;
        public TextMeshProUGUI noListedItems;
        public TextMeshProUGUI playerAccount;
        public UnityEvent OnListed;
        private int index = 0;
        private List<MintedNFT.Response> response;
        public void Init()
        {
            account = PlayerPrefs.GetString("Account");
            description.text = "";
            tokenURI.text = "";
            isApproved.text = "";
            contractAddr.text = "";
            playerAccount.text = account;
            CollecMinted();
        }

        async void CollecMinted()
        {
            try
            {
                response = new List<MintedNFT.Response>();
                response = await EVM.GetMintedNFT(chain, network, account);
                Debug.Log("ITEMS  minted: " + response.Count);

                if (response[index].uri == null)
                {
                    Debug.Log("Not Listed Items");
                    return;
                }
                GetNFTData();
            }
            catch (Exception e)
            {
                noListedItems.text = "NO LISTED ITEM for " + account;
                Debug.Log("No Listed Items" + e);
            }
        }
        async void GetNFTData()
        {
            if (response[index].uri.StartsWith("ipfs://"))
            {
                response[index].uri = response[index].uri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
            }

            UnityWebRequest webRequest = UnityWebRequest.Get(response[index].uri);
            await webRequest.SendWebRequest();
            RootGetNFT data =
                JsonConvert.DeserializeObject<RootGetNFT>(
                    System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            description.text = data.description;
            // parse json to get image uri
            string imageUri = data.image;
            if (imageUri.StartsWith("ipfs://"))
            {
                imageUri = imageUri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
                StartCoroutine(DownloadImage(imageUri));
            }
            else
            {
                StartCoroutine(DownloadImage(imageUri));
            }

            tokenURI.text = response[index].uri;
            Debug.Log(response[index].uri);
            contractAddr.text = response[index].nftContract;
            Debug.Log("NFT Contract: " + response[index].nftContract);
            isApproved.text = response[index].isApproved.ToString();
            _itemID = response[index].id;
            _itemPrice = itemPrice.text;
            Debug.Log("Token Type: " + response[index].tokenType);
            _tokenType = response[index].tokenType;
        }
        public void PrevNFT()
        {
            if (index > 0)
            {
                index--;
                GetNFTData();
            }
        }
        public void NextNFT()
        {
            if (index < response.Count - 1)
            {
                index++;
                GetNFTData();
            }
        }
        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator DownloadImage(string MediaUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else
            {
                Texture2D webTexture = ((DownloadHandlerTexture) request.downloadHandler).texture as Texture2D;
                Sprite webSprite = SpriteFromTexture2D(webTexture);
                textureObject.sprite = webSprite;
            }
        }

        Sprite SpriteFromTexture2D(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
                100.0f);
        }

        public async void ListItem()
        {

            float eth = float.Parse(_itemPrice);
            float decimals = 1000000000000000000; // 18 decimals
            float wei = eth * decimals;
            Debug.Log("ItemID: " + _itemID);
            ListNFT.Response response =
                await EVM.CreateListNftTransaction(chain, network, account, _itemID, Convert.ToDecimal(wei).ToString(), _tokenType);
            Debug.Log("Response: " + response);
            int value = Convert.ToInt32(response.tx.value.hex, 16);
            Debug.Log("Response: " + response);
            try
            {
                string responseNft = await Web3Wallet.SendTransaction(chainID, response.tx.to, value.ToString(),
                    response.tx.data, response.tx.gasLimit, response.tx.gasPrice);
                OnListed.Invoke();
                if (responseNft == null)
                {
                    Debug.Log("Empty Response Object:");
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error: " + e);
            }
        }
    }
}

