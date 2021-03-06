﻿using CoolapkUWP.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoolapkUWP.Models
{
    internal class FeedReplyModel : SimpleFeedReplyModel, INotifyPropertyChanged, ILike
    {
        public FeedReplyModel(JObject token, bool showReplyRow = true) : base(token)
        {
            Dateline = DataHelper.ConvertTime(double.Parse(token["dateline"].ToString().Replace("\"", string.Empty)));
            Message = token.Value<string>("message");
            var userSmallAvatarUrl = token["userInfo"].Value<string>("userSmallAvatar");
            if (!string.IsNullOrEmpty(userSmallAvatarUrl))
                UserSmallAvatar = new ImageModel(userSmallAvatarUrl, ImageType.BigAvatar);
            Likenum = token["likenum"].ToString().Replace("\"", string.Empty);
            Replynum = token["replynum"].ToString().Replace("\"", string.Empty);
            token.TryGetValue("replyRowsCount", out JToken value1);
            ReplyRowsCount = int.Parse(value1?.ToString() ?? "0");
            ShowreplyRows = showReplyRow && ReplyRowsCount > 0;
            if (ShowreplyRows)
            {
                List<SimpleFeedReplyModel> models = new List<SimpleFeedReplyModel>();
                foreach (JObject item in token["replyRows"] as JArray)
                    models.Add(new SimpleFeedReplyModel(item));
                ReplyRows = models.ToArray();
                ReplyRowsMore = token.Value<int>("replyRowsMore");
            }
            Liked = token.TryGetValue("userAction", out JToken v) ? v.Value<int>("like") == 1 : false;
            if (ShowPic)
                Pic = new ImageModel(PicUri, ImageType.SmallImage);
        }

        private string likenum1;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Likenum
        {
            get => likenum1;
            set
            {
                likenum1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Likenum)));
            }
        }

        public string Replynum { get; private set; }
        public new string Message { get; private set; }
        public ImageModel UserSmallAvatar { get; private set; }
        public ImageModel Pic { get; private set; }
        public string Dateline { get; private set; }
        public bool ShowreplyRows { get; set; }
        public SimpleFeedReplyModel[] ReplyRows { get; private set; }
        public bool ShowreplyRowsMore { get => ReplyRowsMore > 0; }
        public double ReplyRowsMore { get; private set; }
        public double ReplyRowsCount { get; private set; }
        public bool Liked { get; set; }
        public bool Liked2 { get => !Liked; }

        string ILike.Id => Id.ToString();
    }
}