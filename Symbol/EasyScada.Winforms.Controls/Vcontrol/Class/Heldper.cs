
using NCalc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyScada;

namespace EasyScada.Winforms.Controls
{
    public class held 
    {
        //EasyScada.Core
        //public static string BackgroundWorker_DoWork2(Control ctr)
        //{

        //    Dictionary<string, object> FuncList2 = fsdfdaf(ctr.Tag.ToString()); // tạo list key
        //    int c2 = 0;
        //    string codegen2 = string.Empty;
        //    if (ctr.Tag != null)
        //    {
        //        foreach (string key2 in FuncList2.Keys)
        //        {
        //            if (FuncList2[key2] is ColorSetting)
        //            {
        //                if (!string.IsNullOrEmpty((FuncList2[key2] as ColorSetting).Expression))
        //                {
        //                    List<string> ltags8 = ExtractFromString((FuncList2[key2] as ColorSetting).Expression, "{", "}");
        //                    foreach (string s16 in ltags8)
        //                    {
        //                        // codegen2 = codegen2 + "//Trigger(\"" + s16 + "\");\n";
        //                        //   easyDriverConnector1.GetTag(s16).ValueChanged += new EventHandler<TagValueChangedEventArgs>(tagchageColorSetting);

        //                    }
        //                    codegen2 += "try\r\n            {";
        //                    if ((FuncList2[key2] as ColorSetting).ColorObjects.Count > 0)
        //                    {
        //                        codegen2 = codegen2 + "int val" + c2.ToString() + "= 0;\r\nint.TryParse(Hten.function.ExeExpression(@\"" + (FuncList2[key2] as ColorSetting).Expression + "\").ToString().Split('.')[0],out val" + c2.ToString() + ");";
        //                        foreach (ColorObject s15 in (FuncList2[key2] as ColorSetting).ColorObjects)
        //                        {
        //                            codegen2 = ((!s15.Value.Contains("-")) ? (codegen2 + "if(val" + c2.ToString() + " == " + s15.Value + ")\r\n                    {\r\n") : (codegen2 + "if(val" + c2.ToString() + " >= " + s15.Value.Split('-')[0] + " && val" + c2.ToString() + " <= " + s15.Value.Split('-')[1] + ")\r\n                    {\r\n"));
        //                            if (s15.Color != Color.Empty)
        //                            {
        //                                codegen2 = codegen2 + "Hten.function.SetProperty(parent,\"" + ctr.Name + "\", \"" + key2 + "\", ColorTranslator.FromHtml(\"" + ColorTranslator.ToHtml(s15.Color) + "\"));\n";
        //                            }
        //                            codegen2 += "}\n";
        //                        }
        //                    }
        //                    codegen2 += "}\n catch { }\n";
        //                }
        //                c2++;
        //            }
        //            if (FuncList2[key2] is StringSetting)
        //            {
        //                if (!string.IsNullOrEmpty((FuncList2[key2] as StringSetting).Expression))
        //                {
        //                    List<string> ltags7 = ExtractFromString((FuncList2[key2] as StringSetting).Expression, "{", "}");
        //                    foreach (string s14 in ltags7)
        //                    {
        //                        codegen2 = codegen2 + "//Trigger(\"" + s14 + "\");\n";
        //                    }
        //                    codegen2 += "try\r\n            {";
        //                    if ((FuncList2[key2] as StringSetting).TextObjects.Count > 0)
        //                    {
        //                        codegen2 = codegen2 + "int val" + c2.ToString() + "= 0;\r\nint.TryParse(Hten.function.ExeExpression(@\"" + (FuncList2[key2] as StringSetting).Expression + "\").ToString().Split('.')[0],out val" + c2.ToString() + ");";
        //                        foreach (StringObject s13 in (FuncList2[key2] as StringSetting).TextObjects)
        //                        {
        //                            codegen2 = ((!s13.Value.Contains("-")) ? (codegen2 + "if(val" + c2.ToString() + " == " + s13.Value + ")\r\n                    {\r\n") : (codegen2 + "if(val" + c2.ToString() + " >= " + s13.Value.Split('-')[0] + " && val" + c2.ToString() + " <= " + s13.Value.Split('-')[1] + ")\r\n                    {\r\n"));
        //                            if (s13.Text != "")
        //                            {
        //                                codegen2 = codegen2 + "Hten.function.SetProperty(parent,\"" + ctr.Name + "\", \"" + key2 + "\", \"" + s13.Text + "\");\n";
        //                            }
        //                            codegen2 += "}\n";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        codegen2 = codegen2 + "object val" + c2.ToString() + " = Hten.function.ExeExpression(@\"" + (FuncList2[key2] as StringSetting).Expression + "\");";
        //                        codegen2 = codegen2 + "Hten.function.SetProperty(parent,\"" + ctr.Name + "\", \"" + key2 + "\", val" + c2.ToString() + ");\n";
        //                    }
        //                    codegen2 += "}\n catch { }\n";
        //                }
        //                c2++;
        //            }
        //            if (FuncList2[key2] is BoolSetting)
        //            {
        //                if (!string.IsNullOrEmpty((FuncList2[key2] as BoolSetting).Expression))
        //                {
        //                    List<string> ltags6 = ExtractFromString((FuncList2[key2] as BoolSetting).Expression, "{", "}");
        //                    foreach (string s12 in ltags6)
        //                    {
        //                        codegen2 = codegen2 + "//Trigger(\"" + s12 + "\");\n";
        //                    }
        //                    codegen2 += "try\r\n            {";
        //                    if ((FuncList2[key2] as BoolSetting).BoolObjects.Count > 0)
        //                    {
        //                        codegen2 = codegen2 + "int val" + c2.ToString() + "= 0;\r\nint.TryParse(Hten.function.ExeExpression(@\"" + (FuncList2[key2] as BoolSetting).Expression + "\").ToString().Split('.')[0],out val" + c2.ToString() + ");";
        //                        foreach (BoolObject s11 in (FuncList2[key2] as BoolSetting).BoolObjects)
        //                        {
        //                            codegen2 = ((!s11.Value.Contains("-")) ? (codegen2 + "if(val" + c2.ToString() + " == " + s11.Value + ")\r\n                    {\r\n") : (codegen2 + "if(val" + c2.ToString() + " >= " + s11.Value.Split('-')[0] + " && val" + c2.ToString() + " <= " + s11.Value.Split('-')[1] + ")\r\n                    {\r\n"));
        //                            if (s11.ControlValue.HasValue)
        //                            {
        //                                codegen2 = codegen2 + "Hten.function.SetProperty(parent,\"" + ctr.Name + "\", \"" + key2 + "\", " + (s11.ControlValue.Value ? "true" : "false") + ");\n";
        //                            }
        //                            codegen2 += "}\n";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        codegen2 = codegen2 + "object val" + c2.ToString() + " = Hten.function.ExeExpression(@\"" + (FuncList2[key2] as BoolSetting).Expression + "\");";
        //                        codegen2 = codegen2 + "bool bval" + c2.ToString() + " = val" + c2.ToString() + ".ToString() ==\"1\";";
        //                        codegen2 = codegen2 + "Hten.function.SetProperty(parent,\"" + ctr.Name + "\", \"" + key2 + "\", bval" + c2.ToString() + ");\n";
        //                    }
        //                    codegen2 += "}\n catch { }\n";
        //                }
        //                c2++;
        //            }
        //            if (FuncList2[key2] is IntSetting)
        //            {
        //                if (!string.IsNullOrEmpty((FuncList2[key2] as IntSetting).Expression))
        //                {
        //                    List<string> ltags5 = ExtractFromString((FuncList2[key2] as IntSetting).Expression, "{", "}");
        //                    foreach (string s10 in ltags5)
        //                    {
        //                        codegen2 = codegen2 + "//Trigger(\"" + s10 + "\");\n";
        //                    }
        //                    codegen2 += "try\r\n            {";
        //                    if ((FuncList2[key2] as IntSetting).IntObjects.Count > 0)
        //                    {
        //                        codegen2 = codegen2 + "int val" + c2.ToString() + "= 0;\r\nint.TryParse(Hten.function.ExeExpression(@\"" + (FuncList2[key2] as IntSetting).Expression + "\").ToString().Split('.')[0],out val" + c2.ToString() + ");";
        //                        foreach (IntObject s9 in (FuncList2[key2] as IntSetting).IntObjects)
        //                        {
        //                            codegen2 = ((!s9.Value.Contains("-")) ? (codegen2 + "if(val" + c2.ToString() + " == " + s9.Value + ")\r\n                    {\r\n") : (codegen2 + "if(val" + c2.ToString() + " >= " + s9.Value.Split('-')[0] + " && val" + c2.ToString() + " <= " + s9.Value.Split('-')[1] + ")\r\n                    {\r\n"));
        //                            codegen2 = codegen2 + "Hten.function.SetProperty(parent,\"" + ctr.Name + "\", \"" + key2 + "\", \"" + s9.Value.ToString() + "\");\n";
        //                            codegen2 += "}\n";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        codegen2 = codegen2 + "object val" + c2.ToString() + " = Hten.function.ExeExpression(@\"" + (FuncList2[key2] as IntSetting).Expression + "\");";
        //                        codegen2 = codegen2 + "int ival" + c2.ToString() + " = 0;";
        //                        codegen2 = codegen2 + "if(int.TryParse( val" + c2.ToString() + ".ToString().Split('.')[0],out ival" + c2.ToString() + ")){";
        //                        codegen2 = codegen2 + "Hten.function.SetProperty(parent,\"" + ctr.Name + "\", \"" + key2 + "\", ival" + c2.ToString() + ");\n}";
        //                    }
        //                    codegen2 += "}\n catch { }\n";
        //                }
        //                c2++;
        //            }
        //        }
        //        return codegen2;
        //    }
        //    return "";

        //    //  type2 = function.ExeCsharpCode(this, allcode)?.type;
        //}


        //public static string BackgroundWorker_DoWork3(Control ctr)
        //{

        //    Dictionary<string, object> FuncList2 = fsdfdaf(ctr.Tag.ToString()); // tạo list key
        //    int c2 = 0;
        //    string codegen2 = string.Empty;
        //    if (ctr.Tag != null)
        //    {
        //        foreach (string key2 in FuncList2.Keys)
        //        {
        //            if (FuncList2[key2] is ColorSetting)
        //            {
        //                if (!string.IsNullOrEmpty((FuncList2[key2] as ColorSetting).Expression))
        //                {
        //                    List<string> ltags8 = ExtractFromString((FuncList2[key2] as ColorSetting).Expression, "{", "}");
        //                    foreach (string s16 in ltags8)
        //                    {
        //                        // codegen2 = codegen2 + "//Trigger(\"" + s16 + "\");\n";
        //                      //   easyDriverConnector1.GetTag(s16).ValueChanged += new EventHandler<TagValueChangedEventArgs>(tagchageColorSetting);

        //                    }
        //                }
        //                c2++;
        //            }
        //            if (FuncList2[key2] is StringSetting)
        //            {
        //                if (!string.IsNullOrEmpty((FuncList2[key2] as StringSetting).Expression))
        //                {
        //                    List<string> ltags7 = ExtractFromString((FuncList2[key2] as StringSetting).Expression, "{", "}");
        //                    foreach (string s14 in ltags7)
        //                    {
        //                        codegen2 = codegen2 + "//Trigger(\"" + s14 + "\");\n";
        //                    }
        //                }
        //                c2++;
        //            }
        //            if (FuncList2[key2] is BoolSetting)
        //            {
        //                if (!string.IsNullOrEmpty((FuncList2[key2] as BoolSetting).Expression))
        //                {
        //                    List<string> ltags6 = ExtractFromString((FuncList2[key2] as BoolSetting).Expression, "{", "}");
        //                    foreach (string s12 in ltags6)
        //                    {
        //                        codegen2 = codegen2 + "//Trigger(\"" + s12 + "\");\n";
        //                    }
        //                }
        //                c2++;
        //            }
        //            if (FuncList2[key2] is IntSetting)
        //            {
        //                if (!string.IsNullOrEmpty((FuncList2[key2] as IntSetting).Expression))
        //                {
        //                    List<string> ltags5 = ExtractFromString((FuncList2[key2] as IntSetting).Expression, "{", "}");
        //                    foreach (string s10 in ltags5)
        //                    {
        //                        codegen2 = codegen2 + "//Trigger(\"" + s10 + "\");\n";
        //                    }                        }
        //                c2++;
        //            }
        //        }
        //        return codegen2;
        //    }
        //    return "";

        //    //  type2 = function.ExeCsharpCode(this, allcode)?.type;
        //}




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

        /// <summary>
        /// Phân tích đưa ra kết quả để so sánh
        /// </summary>
        /// <param name="_expression"></param>
        /// <returns></returns>
        public static object ExeExpression(string _expression) // mục đích để thay thế tag
        {
            string expression = _expression.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            List<string> tags = ExtractFromString(expression, "{", "}"); // mục đích cắt các tag ra
            List<TagExpression> listTag = new List<TagExpression>(); // mục đích lấy giá trị đầu của tag và full name tag
            foreach (string str in tags)
            {
                object val = "0";// GetTag(str);
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
                            return 1;
                        }
                        if (obj.ToString() == false.ToString())
                        {
                            return 0;
                        }
                        return obj;
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


        /// <summary>
        /// Móc ra hết tất cả control nằm trên form
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static List<Control> GetAll(Control control)
        {
            IEnumerable<Control> controls = control.Controls.Cast<Control>();
            List<Control> lst = new List<Control>();
            foreach (Control c in control.Controls)
            {
                if (!string.IsNullOrEmpty(c.Name))
                {
                    lst.Add(c);
                    if (!(c is UserControl))
                    {
                        lst.AddRange(GetAll(c));
                    }
                }
            }
            return lst;
        }
    }


    public class TagExpression
    {
        public string TagFullPath;

        public string TagValue;
    }


}