
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
using System.Reflection;

namespace PropertyCode
{
    public static class held 
    {




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