using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NFTMinted : MonoBehaviour
{
    public GameObject button;
    public Transform parent;

    private string chain = "ethereum";
    private string network = "goerli";
    private string chainID = "5";
    private string account;
    private int index = 0;
    private GameObject tmpButton;
    public void ShowItems()
    {
        account = PlayerPrefs.GetString("Account");
        CollecMinted();
    }
    async void CollecMinted()
    {
        try
        {
            List<MintedNFT.Response> response = await EVM.GetMintedNFT(chain, network, account);
            Debug.Log("ITEMS  minted: " + response.Count);
            if (response.Count > 0)
            {
                for (int i = 0; i < response.Count; i++)
                {
                    tmpButton = Instantiate(button);
                    tmpButton.transform.SetParent(parent);
                    tmpButton.SendMessage("SetLevel", i);
                }
            }
            /*if (response[index].uri == null)
            {
                Debug.Log("Not Listed Items");
                return;
            }
            if (response[index].uri.StartsWith("ipfs://"))
            {
                response[index].uri = response[index].uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
            }

            UnityWebRequest webRequest = UnityWebRequest.Get(response[index].uri);
            await webRequest.SendWebRequest();
            RootGetNFT data =
                JsonConvert.DeserializeObject<RootGetNFT>(
                    System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
            
            // parse json to get image uri
            string imageUri = data.image;
            if (imageUri.StartsWith("ipfs://"))
            {
                imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
                StartCoroutine(DownloadImage(imageUri));
            }
            else
            {
                StartCoroutine(DownloadImage(imageUri));
            }*/

            
        }
        catch (Exception e)
        { 
        
            
            Debug.Log("No Listed Items" + e);
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
            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            Sprite webSprite = SpriteFromTexture2D(webTexture);
            
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
            100.0f);
    }


}
