using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkItem : MonoBehaviour {

	// ID de l'item actuel
	public int itemID;

	// Membre de notre personnage
	public GameObject bodyPart;

	// Liste de nos items (Objets se trouvant dans le membre de notre personnage)
	[SerializeField]
	public List<GameObject> itemList = new List<GameObject>();

	void Update(){
		
		if (transform.childCount > 0)
		{
			itemID = gameObject.GetComponentInChildren<ItemOnObject>().item.itemID;
		}
		else {

			itemID = 0;

			for (int i = 0; i < itemList.Count; i++)
			{
				itemList[i].SetActive(false);
			}
		}

		//Si le jeu détecte plusieurs items dans le membre du personnage on les désactives tous sauf celui ou ceux "réellement équipés"
//		if(bodyPart.transform.childCount > (1 + 5))
//		{
//			print ("t sup ==" + bodyPart.transform.childCount);
//			for (int i = 0; i < itemList.Count; i++)
//			{
//				itemList[i].SetActive(false);
//			}
//		}

		// Copier / Coller le bloc suivant pour chacun de vos items
		// itemID correspond à l'ID de l'item dans la BDD
		// i = X correspond à l'ID (ou index) de l'item dans la liste

		// épée en fer
		//print("ItemID: "+itemID);
		if (itemID == 1 && transform.childCount > 0)
		{
			itemList[0].SetActive(true);
			itemList[1].SetActive(false);
		}
		if (itemID == 4 && transform.childCount > 0)
		{
			itemList[1].SetActive(true);
			itemList[0].SetActive(false);

		}

	}
}
