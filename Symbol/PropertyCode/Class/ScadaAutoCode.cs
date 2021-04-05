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
using EasyScada.Winforms.Controls;

namespace PropertyCode
{
    public  class ScadaAutoCode
    {

        public static Dictionary<string, object> ListCode { get; private set; }
        public static Control control;
        private Control oldControl;// = new Control();
        public static IEasyDriverConnector connector => EasyDriverConnectorProvider.GetEasyDriverConnector();

        #region PropertyAutoChange
        public static void Main(Control ctr, string code )
        {
            control = ctr;
            ListCode = GetDicFromTagCtr(code); // tạo list key

            if (ListCode.Count() > 0)
            {
                foreach (string key in ListCode.Keys)
                {
                    if (ListCode[key] is ColorSetting)
                    {
                        if (!string.IsNullOrEmpty((ListCode[key] as ColorSetting).Expression))
                        {
                            OnTagColorValueChanged(key); //Tạo sự kiện lần đầu khi load
                            var listtagColor = ExtractFromString((ListCode[key] as ColorSetting).Expression, "{", "}");
                            if (listtagColor.Count > 0)
                            {
                                foreach (string tag in listtagColor)
                                {
                                    //tạo sự kiện cho tưng tag 
                                    connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagColorValueChanged(key);
                                    };
                                }

                            }
                        }
                    }
                    if (ListCode[key] is StringSetting)
                    {
                        if (!string.IsNullOrEmpty((ListCode[key] as StringSetting).Expression))
                        {
                            OnTagStringValueChanged(key);
                            var listtagString = ExtractFromString((ListCode[key] as ColorSetting).Expression, "{", "}");
                            if (listtagString.Count > 0)
                            {
                                foreach (string tag in listtagString)
                                {
                                    connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagStringValueChanged(key);
                                    };
                                }

                            }
                        }

                    }
                    if (ListCode[key] is BoolSetting)
                    {
                        if (!string.IsNullOrEmpty((ListCode[key] as BoolSetting).Expression))
                        {
                            OnTagBoolValueChanged(key);
                            var listtagBool = ExtractFromString((ListCode[key] as ColorSetting).Expression, "{", "}");
                            if (listtagBool.Count > 0)
                            {
                                foreach (string tag in listtagBool)
                                {
                                    connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagBoolValueChanged(key);
                                    };
                                }

                            }

                        }
                    }
                    if (ListCode[key] is IntSetting)
                    {
                        if (!string.IsNullOrEmpty((ListCode[key] as IntSetting).Expression))
                        {
                            OnTagIntValueChanged(key);
                            var listtagInt = ExtractFromString((ListCode[key] as ColorSetting).Expression, "{", "}");
                            if (listtagInt.Count > 0)
                            {
                                foreach (string tag in listtagInt)
                                {
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
            List<string> listSt = ExtractFromString(TagControl, "[BEGIN]", "[END]");

            foreach (string s in listSt)
            {
                string key = s.Split(new string[] { "[KEY]" }, StringSplitOptions.None)[0];
                string valu = s.Split(new string[] { "[KEY]" }, StringSplitOptions.None)[1];

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

        private static void OnTagColorValueChanged(string keyPropertyStype)
        {
            string val = ExeExpression((ListCode[keyPropertyStype] as ColorSetting).Expression).Split('.')[0]; //Lấy giá trị hàm biểu thức
            var listCount = (ListCode[keyPropertyStype] as ColorSetting).ColorObjects.Count;
            if (listCount > 0)
            {
                foreach (ColorObject obj in (ListCode[keyPropertyStype] as ColorSetting).ColorObjects)
                {
                    if (obj.Value.Contains("-")) //Khoảng range giá trị
                    {
                        if (Convert.ToInt16(val) >= Convert.ToInt16(obj.Value.Split('-')[0]) &&
                            Convert.ToInt16(val) >= Convert.ToInt16(obj.Value.Split('-')[1]))
                        {
                            SetProperty(control, keyPropertyStype, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
                        }
                        //else if (i = (listCount-1))
                        //{
                        //    SetProperty(control, keyPropertyStype, GetProperty(oldControl, keyPropertyStype));
                        //}

                    }
                    else if (obj.Value.Contains("!")) //Khác giá trị
                    {
                        if (val != obj.Value)
                        {
                            SetProperty(control, keyPropertyStype, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
                        }
                        //else if (i = (listCount-1))
                        //{
                        //    SetProperty(control, keyPropertyStype, GetProperty(oldControl, keyPropertyStype));
                        //}

                    }
                    else
                    {
                        if (val == obj.Value) //Value 
                        {
                            SetProperty(control, keyPropertyStype, ColorTranslator.FromHtml(ColorTranslator.ToHtml(obj.Color)));
                        }
                        //else if (i = (listCount-1))
                        //{
                        //    SetProperty(control, keyPropertyStype, GetProperty(oldControl, keyPropertyStype));
                        //}
                    }

                }
            }
        }


        private static void OnTagIntValueChanged(string keyPropertyStype)
        {
            throw new NotImplementedException();
        }
        private static void OnTagBoolValueChanged(string keyPropertyStype)
        {
        }

        private static void OnTagStringValueChanged(string keyPropertyStype)
        {
        }


        #endregion
        public static string ExeExpression(string _expression) 
        {
            string expression = _expression.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            List<string> tags = ExtractFromString(expression, "{", "}"); // mục đích để thay thế giá trị vào tag bên trong {}
            List<TagExpression> listTag = new List<TagExpression>(); // mục đích lấy giá trị đầu của tag và full name tag
            foreach (string nameTagCur in tags)
            {
                string nameTag = "";
                if (nameTagCur.Contains("@NOP::")) //Nếu đinh dạng có @NOP:: thì bỏ qua tag Prefix
                {
                    nameTag = nameTagCur;

                }
                else   //getParent lấy tagprefix nối vào
                {
                    nameTag = nameTagCur;// getparentTagPrefix + nameTagCur;
                }
                //////////////////////Get tag///////////////////////////
                object valueTag =null;
                if (connector.GetTag(nameTag)!=null)
                {
                    valueTag = connector.GetTag(nameTag).Value;
                }

                listTag.Add(new TagExpression
                {
                    TagFullPath = nameTag,
                    TagValue = ((valueTag == null) ? "0" : valueTag.ToString())
                });
            }


            string Expression = expression;
            foreach (TagExpression str2 in listTag) // mục đích thế các tagfull và giá trj vào
            {
                Expression = Expression.Replace("{" + str2.TagFullPath + "}", str2.TagValue);
            }
            if (expression.Contains("{") && expression.ToString().Contains("}") && expression.StartsWith("{")
                && expression.ToString().EndsWith("}") && expression.ToString().Split('{').Length == 2
                && expression.ToString().Split('}').Length == 2)
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
