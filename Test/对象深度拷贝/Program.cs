using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace 对象深度拷贝
{
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A(2);
            a.g1 = "asd";
            a.g2 = "qwe";
            B b = new B();
            b.nm = "iop";
            a.g4.Add(b);
            A a1 = new A();
            //a1 =(A)CopyOjbect(a);
            //CopyValueToTarget(a, a1);
            //a1 = BinaryClone(a);
            a1 = XMLClone(a);
            Console.WriteLine(a1.g1 + "##" + a1.g2 + "##" + a1.getg3());
            foreach (var item in a1.g4)
            {
                Console.WriteLine(item.nm);
            }
            Console.WriteLine("=============");
            B b1 = new B();
            b1.nm = "icp";
            a.g4.Add(b1);
            Console.WriteLine(a1.g1 + "##" + a1.g2 + "##" + a1.getg3());
            foreach (var item in a1.g4)
            {
                Console.WriteLine(item.nm);
            }
        }
        /// <summary>
        /// 对象拷贝 公用字段、属性的深度拷贝  缺点：遇到想List<T>自定义类的集合，无法深度复制
        /// </summary>
        /// <param name="obj">被复制对象</param>
        /// <returns>新对象</returns>
        private static object CopyOjbect(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            Object targetDeepCopyObj;
            Type targetType = obj.GetType();
            //值类型  
            if (targetType.IsValueType == true)
            {
                targetDeepCopyObj = obj;
            }
            //引用类型   
            else
            {
                targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象   
                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    //拷贝字段
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, CopyOjbect(fieldValue));
                        }

                    }//拷贝属性
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;

                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            try
                            {
                                object propertyValue = myProperty.GetValue(obj, null);
                                if (propertyValue is ICloneable)
                                {
                                    myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                                }
                                else
                                {
                                    myProperty.SetValue(targetDeepCopyObj, CopyOjbect(propertyValue), null);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            return targetDeepCopyObj;
        }

        /// <summary>
        /// 私有和公用字段、属性的深度拷贝  缺点：遇到想List<T>自定义类的集合，List是无法深度copy的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private static void CopyValueToTarget<T>(T source, T target) where T : class
        {
            Type type = source.GetType();
            var fields = type.GetRuntimeFields().ToList();
            foreach (var field in fields)
            {
                field.SetValue(target, field.GetValue(source));
            }

            var properties = type.GetRuntimeProperties().ToList();
            foreach (var property in properties)
            {
                property.SetValue(target, property.GetValue(source));
            }
        }

        /// <summary>
        /// 二进制序列化的深度克隆 缺点：所有涉及到的类都要是[Serializable],且私有的属性与字段不能深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T BinaryClone<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
        /// <summary>
        /// Xml形式序列化的深度克隆  缺点：私有的属性与字段不能深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T XMLClone<T>(T RealObject)
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, RealObject);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);
            }
        }

    }
    public class A
    {
        public string g1 { set; get; }
        public string g2;
        private int g3;
        public List<B> g4 = new List<B>();
        public A(int ji)
        {
            g3 = ji;
        }
        public int getg3()
        {
            return g3;
        }
        public A() { }
    }

    public class B:ICloneable
    {
        public string nm = "kl";

        public object Clone()
        {
            return nm;
        }
    }
    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        //当然前题是List中的对象要实现ICloneable接口
    } 
}
