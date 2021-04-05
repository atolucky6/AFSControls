using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using NCalc;
using Newtonsoft.Json;
using System.Security.AccessControl;
using EasyScada.Core;

namespace EasyScada.Winforms.Controls
{
    public static class ScadaAutoCode
    {

        public static Dictionary<string, object> FuncList2 { get; private set; }
        public static Control control;
        public static IEasyDriverConnector connector;


        #region PropertyAutoChange
        public static void Main(IEasyDriverConnector Connector, Control ctr)
        {
            connector = Connector;
            control = ctr;
            FuncList2 = GetDicFromTagCtr(control.Tag.ToString()); // tạo list key

            if (control.Tag != null)
            {
                foreach (string key in FuncList2.Keys)
                {
                    if (FuncList2[key] is ColorSetting)
                    {
                        if (!string.IsNullOrEmpty((FuncList2[key] as ColorSetting).Expression))
                        {
                            OnTagColorValueChanged(key);
                            var listtagColor = ExtractFromString((FuncList2[key] as ColorSetting).Expression, "{", "}");
                            if (listtagColor.Count > 0)
                            {
                                foreach (string tag in listtagColor)
                                {
                                    //tạo sự kiện cho tưng tag //  Connector.GetTag(tag).ValueChanged += OnTagColorValueChanged;
                                    connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagColorValueChanged(key);
                                    };
                                }

                            }
                        }
                    }
                    if (FuncList2[key] is StringSetting)
                    {
                        if (!string.IsNullOrEmpty((FuncList2[key] as StringSetting).Expression))
                        {
                            OnTagStringValueChanged(key);
                            var listtagString = ExtractFromString((FuncList2[key] as ColorSetting).Expression, "{", "}");
                            if (listtagString.Count > 0)
                            {
                                foreach (string tag in listtagString)
                                {
                                    //tạo sự kiện cho tưng tag //  Connector.GetTag(tag).ValueChanged += OnTagColorValueChanged;
                                    connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagStringValueChanged(key);
                                    };
                                }

                            }
                        }

                    }
                    if (FuncList2[key] is BoolSetting)
                    {
                        if (!string.IsNullOrEmpty((FuncList2[key] as BoolSetting).Expression))
                        {
                            OnTagBoolValueChanged(key);
                            var listtagBool = ExtractFromString((FuncList2[key] as ColorSetting).Expression, "{", "}");
                            if (listtagBool.Count > 0)
                            {
                                foreach (string tag in listtagBool)
                                {
                                    //tạo sự kiện cho tưng tag //  Connector.GetTag(tag).ValueChanged += OnTagColorValueChanged;
                                    connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagBoolValueChanged(key);
                                    };
                                }

                            }

                        }
                    }
                    if (FuncList2[key] is IntSetting)
                    {
                        if (!string.IsNullOrEmpty((FuncList2[key] as IntSetting).Expression))
                        {
                            OnTagIntValueChanged(key);
                            var listtagInt = ExtractFromString((FuncList2[key] as ColorSetting).Expression, "{", "}");
                            if (listtagInt.Count > 0)
                            {
                                foreach (string tag in listtagInt)
                                {
                                    //tạo sự kiện cho tưng tag //  Connector.GetTag(tag).ValueChanged += OnTagColorValueChanged;
                                    connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagIntValueChanged(key);
                                    };
                                }

                            }


                        }
                    }
                }
            }
        }



        /// <summary>
        /// phân tích Tag ra list dic
        /// </summary>
        /// <param name="TagControl"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetDicFromTagCtr(string TagControl)
        {
            Dictionary<string, object> FuncList = new Dictionary<string, object>();
            //  old = refCOntrol.Tag.ToString();
            List<string> listSt = ExtractFromString(TagControl, "[STX]", "[ETX]");

            foreach (string s in listSt)
            {
                string key = s.Split(new string[] { "[====]" }, StringSplitOptions.None)[0];
                string valu = s.Split(new string[] { "[====]" }, StringSplitOptions.None)[1];

                if (key.Contains("ColorSetting"))
                {
                    ColorSetting obj = JsonConvert.DeserializeObject<ColorSetting>(valu);
                    FuncList.Add(key.Split('_')[0], obj);
                }
                if (key.Contains("StringSetting"))
                {
                    StringSetting obj = JsonConvert.DeserializeObject<StringSetting>(valu);
                    FuncList.Add(key.Split('_')[0], obj);
                }
                if (key.Contains("BoolSetting"))
                {
                    BoolSetting obj = JsonConvert.DeserializeObject<BoolSetting>(valu);
                    FuncList.Add(key.Split('_')[0], obj);
                }
                if (key.Contains("IntSetting"))
                {
                    IntSetting obj = JsonConvert.DeserializeObject<IntSetting>(valu);
                    FuncList.Add(key.Split('_')[0], obj);
                }
            }
            return FuncList;

        }

        /// <summary>
        /// Phân tích sau đó lấy ra 1 list tag
        /// </summary>
        /// <param name="_expression"></param>
        /// <returns></returns>
        public static List<string> ExtractFromString(string text, string startString, string endString)
        {
            List<string> matched = new List<string>();
            int indexStart2 = 0;
            int indexEnd2 = 0;
            bool exit = false;
            while (!exit)
            {
                indexStart2 = text.IndexOf(startString);
                indexEnd2 = text.IndexOf(endString);
                if (indexEnd2 < indexStart2)
                {
                    string textkk = text.Substring(indexStart2);
                    indexEnd2 = indexStart2 + textkk.IndexOf(endString);
                }
                if (indexStart2 != -1 && indexEnd2 != -1)
                {
                    matched.Add(text.Substring(indexStart2 + startString.Length, indexEnd2 - indexStart2 - startString.Length));
                    text = text.Substring(indexEnd2 + endString.Length);
                }
                else
                {
                    exit = true;
                }
            }
            return matched;
        }



        private static void OnTagIntValueChanged(string key)
        {
            throw new NotImplementedException();
        }
        private static void OnTagBoolValueChanged(string key)
        {
            throw new NotImplementedException();
        }

        private static void OnTagStringValueChanged(string key)
        {
            ////lấy giá trị đầu tiên
            //string val = ExeExpression((FuncList2[key] as ColorSetting).Expression).Split('.')[0]; // đọc giá trị sau đó thế vào string rồi so sánh trả ra string
            //if ((FuncList2[key] as ColorSetting).ColorObjects.Count > 0)
            //{
            //    foreach (ColorObject obj in (FuncList2[key] as ColorSetting).ColorObjects)
            //    {
            //        if (obj.Value.Contains("-"))
            //        {
            //            if (Convert.ToInt16(val) >= Convert.ToInt16(obj.Value.Split('-')[0]) &&
            //                Convert.ToInt16(val) >= Convert.ToInt16(obj.Value.Split('-')[1]))
            //            {
            //                SetProperty(this, key, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
            //            }

            //        }
            //        else if (obj.Value.Contains("!"))
            //        {
            //            if (Convert.ToInt16(val) != Convert.ToInt16(obj.Value))
            //            {
            //                SetProperty(this, key, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
            //            }

            //        }
            //        else
            //        {
            //            if (Convert.ToInt16(val) == Convert.ToInt16(obj.Value))
            //            {
            //                SetProperty(this, key, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
            //            }

            //        }
            //    }
            //}
        }

        private static void OnTagColorValueChanged(string key)
        {
            //lấy giá trị đầu tiên
            string val = ExeExpression((FuncList2[key] as ColorSetting).Expression).Split('.')[0]; // đọc giá trị sau đó thế vào string rồi so sánh trả ra string
            if ((FuncList2[key] as ColorSetting).ColorObjects.Count > 0)
            {
                ColorSetting d = FuncList2[key] as ColorSetting;
                foreach (ColorObject obj in (FuncList2[key] as ColorSetting).ColorObjects)
                {
                    if (obj.Value.Contains("-"))
                    {
                        if (Convert.ToInt16(val) >= Convert.ToInt16(obj.Value.Split('-')[0]) &&
                            Convert.ToInt16(val) >= Convert.ToInt16(obj.Value.Split('-')[1]))
                        {
                            SetProperty(control, key, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
                        }
                        //else
                        //{
                        //    SetProperty(this, key, GetProperty(controlStatup, key));
                        //}
                        //return;

                    }
                    else if (obj.Value.Contains("!"))
                    {
                        if (val != obj.Value)
                        {
                            SetProperty(control, key, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
                        }
                        //else
                        //{
                        //    SetProperty(this, key, GetProperty(controlStatup, key));
                        //}
                        //return;

                    }
                    else
                    {
                        if (val == obj.Value)
                        {
                            SetProperty(control, key, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
                            // return;
                        }
                        //else
                        //{
                        //    SetProperty(this, key, GetProperty(controlStatup, key));
                        //    return;
                        //}
                    }
                }
            }
        }

        #endregion
        public static string ExeExpression(string _expression) // mục đích để thay thế tag
        {
            string expression = _expression.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            List<string> tags = ExtractFromString(expression, "{", "}"); // mục đích cắt các tag ra
            List<TagExpression> listTag = new List<TagExpression>(); // mục đích lấy giá trị đầu của tag và full name tag
            foreach (string str in tags)
            {
                object val = connector.GetTag(str).Value;
                listTag.Add(new TagExpression
                {
                    TagFullPath = str,
                    TagValue = ((val == null) ? "0" : val.ToString())
                });
            }


            string Expression = expression;
            foreach (TagExpression str2 in listTag) // mục đích thế các tagfull và giá trj vào
            {
                Expression = Expression.Replace("{" + str2.TagFullPath + "}", str2.TagValue);
            }
            if (expression.Contains("{") && expression.ToString().Contains("}") && expression.StartsWith("{") && expression.ToString().EndsWith("}") && expression.ToString().Split('{').Length == 2 && expression.ToString().Split('}').Length == 2)
            {
                return Expression;
            }
            Expression ex = new Expression(Expression, EvaluateOptions.NoCache);
            if (!ex.HasErrors())
            {
                if (ex.ParsedExpression.ToString().Contains("{") && ex.ParsedExpression.ToString().Contains("}"))
                {
                    if (ex.ParsedExpression.ToString().StartsWith("{") && ex.ParsedExpression.ToString().EndsWith("}") && ex.ParsedExpression.ToString().Split('{').Length == 2 && ex.ParsedExpression.ToString().Split('}').Length == 2)
                    {
                        return expression;
                    }
                }
                else
                {
                    try
                    {
                        object obj = ex.Evaluate();
                        if (obj.ToString() == true.ToString())
                        {
                            return "1";
                        }
                        if (obj.ToString() == false.ToString())
                        {
                            return "0";
                        }
                        return obj.ToString();
                    }
                    catch
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                return "0";
            }
            return "0";
        }


        public static void SetProperty(Control control, string propertyName, object value)
        {
            //  this.SetInvoke((x) =>

            control.SetInvoke((x) =>
            {
                PropertyDescriptor pd = TypeDescriptor.GetProperties(control)[propertyName];
                pd.SetValue(control, value);
            });

        }
        public static object GetProperty(Control control, string propertyName)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(control)[propertyName];
            return pd.GetValue(control);
        }


    }
}
