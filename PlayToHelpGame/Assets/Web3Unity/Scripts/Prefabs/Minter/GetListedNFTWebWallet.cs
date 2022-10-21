using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GetListedNFTWebWallet : MonoBehaviour
{
    private string chain = "ethereum";
    public Image nftImage;
    private string network = "goerli";
    private string chainID = "5";
    public TextMeshProUGUI price;
    public TextMeshProUGUI seller;
    public TextMeshProUGUI description;
    public TextMeshProUGUI listPercentage;
    public TextMeshProUGUI contractAddr;
    public TextMeshProUGUI tokenId;
    public TextMeshProUGUI itemId;
    public UnityEvent OnPurchased;
    private string _itemPrice = "";
    private string _tokenType = "";

    private string _itemID = "";
    private int index;
    private List<GetNftListModel.Response> response;
    public void Awake()
    {
        
        price.text = "";
        seller.text = "";
        description.text = "";
        listPercentage.text = "";
        tokenId.text = "";
        itemId.text = "";
        contractAddr.text = "";
    }


    // Start is called before the first frame update
    async public void PullData()
    {
        response = new List<GetNftListModel.Response>();
        response = await EVM.GetNftMarket(chain, network);
        index = 0;
        if(response.Count > 0)
        GetNFTData();
    }  

    async void GetNFTData()
    {
        price.text = response[index].price;
        seller.text = response[index].seller;
        Debug.Log("Seller: " + response[index].seller);
        if (response[index].uri.StartsWith("ipfs://"))
        {
            response[index].uri = response[index].uri.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
            Debug.Log("Response URI" + response[index].uri);
        }

        UnityWebRequest webRequest = UnityWebRequest.Get(response[index].uri);
        await webRequest.SendWebRequest();
        RootGetNFT data =
            JsonConvert.DeserializeObject<RootGetNFT>(
                System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        if (data.description == null)
        {
            description.text = "";
        }
        else
        {
            description.text = data.description;
        }

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

        if (data.properties != null)
        {
            foreach (var prop in data.properties.additionalFiles)
            {
                if (prop.StartsWith("ipfs://"))
                {
                    var additionalURi = prop.Replace("ipfs://", "https://ipfs.chainsafe.io/ipfs/");
                }
            }
        }
        listPercentage.text = response[index].listedPercentage;
        Debug.Log(response[index].listedPercentage);
        contractAddr.text = response[index].nftContract;
        itemId.text = response[index].itemId;
        _itemID = response[index].itemId;
        _itemPrice = response[index].price;
        _tokenType = response[index].tokenType;
        tokenId.text = response[index].tokenId;
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
        if (index < response.Count-1)
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
            nftImage.sprite = webSprite;
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
            100.0f);
    }

    public async void PurchaseItem()
    {
        Debug.Log(_itemID);
        Debug.Log(_itemPrice);
        Debug.Log(_tokenType);
        BuyNFT.Response response = await EVM.CreatePurchaseNftTransaction(chain, network,
            PlayerPrefs.GetString("Account"), _itemID, _itemPrice, _tokenType);
        Debug.Log("Account: " + response.tx.account);
        Debug.Log("To : " + response.tx.to);
        Debug.Log("Value : " + response.tx.value);
        Debug.Log("Data : " + response.tx.data);
        Debug.Log("Gas Price : " + response.tx.gasPrice);
        Debug.Log("Gas Limit : " + response.tx.gasLimit);

        try
        {
            
            string responseNft = await Web3Wallet.SendTransaction(chainID, response.tx.to, response.tx.value,
                response.tx.data, response.tx.gasLimit, response.tx.gasPrice);
            OnPurchased.Invoke();
            if (responseNft == null)
            {
                Debug.Log("Empty Response Object:");
            }
            Debug.Log(responseNft);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}

