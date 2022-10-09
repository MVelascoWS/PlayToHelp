using System.Collections;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class MListedNFT : MonoBehaviour
{
    private string chain = "ethereum";
    public Image imageNFT;
    private string network = "goerli";
    public TextMeshProUGUI price;
    public TextMeshProUGUI seller;
    public TextMeshProUGUI description;
    public TextMeshProUGUI listPercentage;
    public TextMeshProUGUI contractAddr;
    public TextMeshProUGUI tokenId;
    public TextMeshProUGUI itemId;
    private string _itemPrice = "";
    private string _tokenType = "";
    private string _itemID = "";
    private int NFTNumber = 0;
    private void Awake()
    {
        price.text = "";
        seller.text = "";
        description.text = "";
        listPercentage.text = "";
        tokenId.text = "";
        itemId.text = "";
        contractAddr.text = "";
    }
   void Start()
    {
        PullNFTData();
    }

    public void Next()
    {
        NFTNumber++;
        PullNFTData();
    }
    public void Prev()
    {
        if(NFTNumber>0)
            NFTNumber--;
        PullNFTData();
    }
    async void PullNFTData()
    {
        List<GetNftListModel.Response> response = await EVM.GetNftMarket(chain, network);
        price.text = response[NFTNumber].price;
        seller.text = response[NFTNumber].seller;
        if (response[NFTNumber].uri.StartsWith("ipfs://"))
        {
            response[NFTNumber].uri = response[NFTNumber].uri.Replace("ipfs://", "https://ipfs.io/ipfs/");
            Debug.Log("Response URI" + response[NFTNumber].uri);
        }

        UnityWebRequest webRequest = UnityWebRequest.Get(response[NFTNumber].uri);
        await webRequest.SendWebRequest();
        RootGetNFT data =
            JsonConvert.DeserializeObject<RootGetNFT>(
                System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        description.text = data.description;
        // parse json to get image uri
        string imageUri = data.image;
        if (imageUri.StartsWith("ipfs://"))
        {
            imageUri = imageUri.Replace("ipfs://", "https://ipfs.io/ipfs/");
            StartCoroutine(DownloadImage(imageUri));
        }

        if (data.properties != null)
        {
            foreach (var prop in data.properties.additionalFiles)
            {
                if (prop.StartsWith("ipfs://"))
                {
                    var additionalURi = prop.Replace("ipfs://", "https://ipfs.io/ipfs/");
                }
            }
        }
        listPercentage.text = response[NFTNumber].listedPercentage;
        contractAddr.text = response[NFTNumber].nftContract;
        itemId.text = response[NFTNumber].itemId;
        _itemID = response[NFTNumber].itemId;
        _itemPrice = response[NFTNumber].price;
        _tokenType = response[NFTNumber].tokenType;
        tokenId.text = response[NFTNumber].tokenId;
    }
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
            imageNFT.sprite = webSprite;
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
            100.0f);
    }
}
