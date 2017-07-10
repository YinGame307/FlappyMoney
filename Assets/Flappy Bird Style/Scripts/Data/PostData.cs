//
// PostData.cs
//
// Author:
//       Yin <>
//
//
//
using System;
using System.Collections.Generic;
using System.Collections;

public class PostData
{
	public string action;
	public Dictionary<string, Object> data;

	public PostData ()
	{
	}
	public PostData (string action)
	{
		this.action = action;
		data = new Dictionary<string, Object> ();
	}
	public string toString(){
		return LitJson.JsonMapper.ToJson (this);
	}
}


