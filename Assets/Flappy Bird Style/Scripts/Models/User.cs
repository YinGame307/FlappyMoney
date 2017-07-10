using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User{
	
	public int id;
	public string secretKey;
	public int userGroup;
	public int userLevel;
	public int avaiableCoin;
	public int pendingCoin;
	public int updateTime;
	public int createdTime;

	//Facebook
	public string userName;
	public string name;
	public string token;

	public string FCM_Token;

	public User(){
		
	}

	public User(int id, string name, string token){
		this.id = id;
		this.name = name;
		this.token = token;
	}


}
