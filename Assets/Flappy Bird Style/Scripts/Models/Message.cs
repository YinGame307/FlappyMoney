//
// Message.cs
//
// Author:
//       Yin <>
//
//
//
using System;


public class Message
{
	public int id;
	public string title;
	public string content;
	public int is_read;
	public Message ()
	{
	}

	public bool isRead(){
		return is_read == 1;
	}

	public void readMessage(){
		is_read = 1;
	}
}


