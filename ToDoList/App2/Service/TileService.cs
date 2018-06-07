using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using App2.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace App2.Service
{
    public class TitleService
    {

        public static Work Item = new Work();
        
        public TileContent tileContent = new TileContent()
        {
            Visual = new TileVisual()
            {
                TileSmall = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        TextStacking = TileTextStacking.Center,
                        BackgroundImage = new TileBackgroundImage()
                        {
                            Source = "Assets/Square150x150Logo.scale-200.png"
                        },
                        Children =
                {
                    new AdaptiveText()
                    {
                        Text = Item.Title,
                        HintAlign = AdaptiveTextAlign.Center
                    }
                }
                    }
                },
                TileMedium = new TileBinding()
                {
                    Branding = TileBranding.Name,
                    DisplayName = Item.Date.ToString(),
                    Content = new TileBindingContentAdaptive()
                    {
                        BackgroundImage = new TileBackgroundImage()
                        {
                            Source = "Assets/Square150x150Logo.scale-200.png"
                        },
                        Children =
                {
                    new AdaptiveText()
                    {
                        Text = Item.Title,
                        HintWrap = true,
                        HintMaxLines = 2
                    },
                    new AdaptiveText()
                    {
                        Text = Item.Detail,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
                    }
                },
                TileWide=new TileBinding()
                {
                    Branding = TileBranding.NameAndLogo,
                    DisplayName = "TODOList",
                    Content = new TileBindingContentAdaptive()
                    {
                        BackgroundImage = new TileBackgroundImage()
                        {
                            Source = "Assets/Square150x150Logo.scale-200.png"
                        },
                        Children =
                {
                    new AdaptiveText()
                    {
                        Text = Item.Title
                    },
                    new AdaptiveText()
                    {
                        Text = Item.Detail,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText()
                    {
                        Text = Item.Date.ToString(),
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
                    }
                },
                TileLarge = new TileBinding()
                {
                    Branding = TileBranding.NameAndLogo,
                    DisplayName = "TODOList",
                    Content = new TileBindingContentAdaptive()
                    {
                        BackgroundImage = new TileBackgroundImage()
                        {
                            Source = "Assets/Square150x150Logo.scale-200.png"
                        },
                        Children =
                {
                    new AdaptiveText()
                    {
                        Text = Item.Title
                    },
                    new AdaptiveText()
                    {
                        Text = Item.Detail,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText()
                    {
                        Text = Item.Date.ToString(),
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
                    }
                }
            }
        };
    }
}
