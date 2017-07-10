//
// LoginResult.cs
//
// Author:
//       Yin <>
//
//
//
using System;


public class LoginResult
{
	public bool status;
	public string message;
	public Data data;

	public LoginResult ()
	{
	}


	public class Data{

		public Info info;
		public Notification[] notifications;

		public class Info{
			public int id;
			public string user_name;
			public string fb_token;
			public string secret_key;
			public int user_group;
			public int user_level;
			public int available_coin;
			public int pending_coin;

			public int update_time;
			public int created_time;

			public Info(){
			
			}
		}

		public int getUnreadNotifi(){
			int res = 0;
			foreach (Notification notification in notifications) {
				if (notification.is_read == 1) {
					res++;
				}
			}
			return res;
		}

		public class Notification{
			public int id;
			public int uid;
			public string title;
			public string content;
			public int is_read;
		}
	}

}


