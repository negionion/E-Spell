using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour {
	public GameObject preObj;		//預製物件
	[SerializeField]
	private GameObject parentObj;   //放置在哪個物件底下
	public int poolSize = 10;
	private Queue<GameObject> pool = new Queue<GameObject>();

	// Use this for initialization
	void Awake () {
		creatObj();
	}

	//產生物件池中的物件
	private void creatObj()
	{
		GameObject _obj;
		for (int i = 0; i < poolSize; i++)
		{
			_obj = Instantiate(preObj);
			_obj.SetActive(false);
			_obj.transform.SetParent(parentObj.transform, false);
			pool.Enqueue(_obj);
		}
	}
	
	//取出物件
	public GameObject reuse(Vector3 iniPosition)
	{
		if (pool.Count == 0)
			creatObj();
		GameObject _reuse = pool.Dequeue();                                                  
		_reuse.transform.position = iniPosition;	//定義物件位置
		_reuse.SetActive(true);                     //顯示物件 
		return _reuse;
	}
	public GameObject reuse(Vector3 iniPosition, Quaternion iniRotation)
	{
		if (pool.Count == 0)
			creatObj();
		GameObject _reuse = pool.Dequeue();
		_reuse.transform.position = iniPosition;    //定義物件位置
		_reuse.transform.rotation = iniRotation;	//定義物件旋轉角
		_reuse.SetActive(true);                     //顯示物件 
		return _reuse;
	}

	//回收物件，使用下方註解回收物件
	//GameObject.Find("掛載ObjPool的物件名稱").GetComponent<"Pool的腳本名稱">().recovery(gameObject);
	//GetComponentInParent<"Pool的腳本名稱">().recovery(gameObject);				//作為池的子物件時
	public void recovery(GameObject reObj)   //回收物件
	{
		pool.Enqueue(reObj);
		reObj.SetActive(false);
	}

}
