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

namespace EasyScada.Winforms.Controls.Vcontrol.Class
{
    class ScadaAutoCode
    {
        public Dictionary<string, object> FuncList2 { get; private set; }
        private List<string> listtagInt;
        private List<string> listtagBool;
        private List<string> listtagString;
        private List<string> listtagColor;

        #region PropertyAutoChange
        private void BackgroundWorker_DoWork4(IEasyDriverConnector Connector,Control ctr)
        {
            FuncList2 = held.fsdfdaf(ctr.Tag.ToString()); // tạo list key
            if (ctr.Tag != null)
            {
                foreach (string key in FuncList2.Keys)
                {
                    if (FuncList2[key] is ColorSetting)
                    {
                        if (!string.IsNullOrEmpty((FuncList2[key] as ColorSetting).Expression))
                        {
                            OnTagColorValueChanged(ctr,FuncList2, key);
                            listtagColor = held.ExtractFromString((FuncList2[key] as ColorSetting).Expression, "{", "}");
                            if (listtagColor.Count > 0)
                            {
                                foreach (string tag in listtagColor)
                                {
                                    //tạo sự kiện cho tưng tag //  Connector.GetTag(tag).ValueChanged += OnTagColorValueChanged;
                                    Connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagColorValueChanged(ctr,FuncList2, key);
                                    };
                                }

                            }
                        }
                    }
                    if (FuncList2[key] is StringSetting)
                    {
                        if (!string.IsNullOrEmpty((FuncList2[key] as StringSetting).Expression))
                        {
                            OnTagStringValueChanged(ctr,FuncList2, key);
                            listtagString = held.ExtractFromString((FuncList2[key] as ColorSetting).Expression, "{", "}");
                            if (listtagString.Count > 0)
                            {
                                foreach (string tag in listtagString)
                                {
                                    //tạo sự kiện cho tưng tag //  Connector.GetTag(tag).ValueChanged += OnTagColorValueChanged;
                                    Connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagStringValueChanged(ctr,FuncList2, key);
                                    };
                                }

                            }
                        }

                    }
                    if (FuncList2[key] is BoolSetting)
                    {
                        if (!string.IsNullOrEmpty((FuncList2[key] as BoolSetting).Expression))
                        {
                            OnTagBoolValueChanged(ctr,FuncList2, key);
                            listtagBool = held.ExtractFromString((FuncList2[key] as ColorSetting).Expression, "{", "}");
                            if (listtagBool.Count > 0)
                            {
                                foreach (string tag in listtagBool)
                                {
                                    //tạo sự kiện cho tưng tag //  Connector.GetTag(tag).ValueChanged += OnTagColorValueChanged;
                                    Connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagBoolValueChanged(ctr,FuncList2, key);
                                    };
                                }

                            }

                        }
                    }
                    if (FuncList2[key] is IntSetting)
                    {
                        if (!string.IsNullOrEmpty((FuncList2[key] as IntSetting).Expression))
                        {
                            OnTagIntValueChanged(ctr,FuncList2, key);
                            listtagInt = held.ExtractFromString((FuncList2[key] as ColorSetting).Expression, "{", "}");
                            if (listtagInt.Count > 0)
                            {
                                foreach (string tag in listtagInt)
                                {
                                    //tạo sự kiện cho tưng tag //  Connector.GetTag(tag).ValueChanged += OnTagColorValueChanged;
                                    Connector.GetTag(tag).ValueChanged += (x, y) =>
                                    {
                                        OnTagIntValueChanged(ctr,FuncList2, key);
                                    };
                                }

                            }


                        }
                    }
                }
            }
        }

        private void OnTagIntValueChanged(Control control, Dictionary<string, object> funcList2, string key)
        {
            throw new NotImplementedException();
        }

        private void OnTagBoolValueChanged(Control control, Dictionary<string, object> funcList2, string key)
        {
            throw new NotImplementedException();
        }

        private void OnTagStringValueChanged(Control control, Dictionary<string, object> funcList2, string key)
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

        private void OnTagColorValueChanged(Control control, Dictionary<string, object> funcList2, string key)
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

        public void SetProperty(Control control, string propertyName, object value)
        {
          //  this.SetInvoke((x) =>

            control.SetInvoke((x) =>
            {
                PropertyDescriptor pd = TypeDescriptor.GetProperties(control)[propertyName];
                pd.SetValue(control, value);
            });

        }
        public object GetProperty(Control control, string propertyName)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(control)[propertyName];
            return pd.GetValue(control);
        }

        #endregion
        public string ExeExpression(string _expression) // mục đích để thay thế tag
        {
            string expression = _expression.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            List<string> tags = held.ExtractFromString(expression, "{", "}"); // mục đích cắt các tag ra
            List<TagExpression> listTag = new List<TagExpression>(); // mục đích lấy giá trị đầu của tag và full name tag
            foreach (string str in tags)
            {
                object val = Connector.GetTag(str).Value;
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

    }
}
