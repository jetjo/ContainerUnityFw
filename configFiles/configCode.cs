//using Microsoft.Practices.Unity.Configuration;
//using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Configuration;
using Microsoft.Practices.Unity.GuardSupport;
using Microsoft.Practices.Unity.GuardSupport.Configuration;

namespace UnityContainerr
{

    using Microsoft.Practices.Unity.Configuration.Tests.TestObjects;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Unity.Injection;
    using Microsoft.Practices.Unity.Configuration.Tests.TestObjects.MyGenericTypes;

    /// <summary>
    /// 班级接口
    /// </summary>
    public interface IClass
    {
        string ClassName { get; set; }

        void ShowInfo();
    }
    /// <summary>
    /// 计科班
    /// </summary>
    public class CbClass : IClass
    {
        public string ClassName { get; set; }

        public void ShowInfo()
        {
            Console.WriteLine("计科班：{0}", ClassName);
        }
    }
    /// <summary>
    /// 电商班
    /// </summary>
    public class EcClass : IClass
    {
        public string ClassName { get; set; }

        public void ShowInfo()
        {
            Console.WriteLine("电商班：{0}", ClassName);
        }
    }

    public class ContainerFactory
    {
        static void Main(string[] args)
        {
            ContainerConfiguration<ContainerFactory>();
        }

        //[TestMethod]
        public static IUnityContainer ContainerConfiguration<TResourceLocator>(string unityConfigFileName="unity.config",string unitySectionName="unity"
            ,string unityContainerName="unityContainer")
        {
            IUnityContainer unityContainer = new UnityContainer();

            #region unity.config

            /*
            //string configFile = "unity.config";
            //var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };

            ////从config文件中读取配置信息
            //Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            ////获取指定名称的配置节

            ////var test = configuration.GetSection("unity");

            ////var testType = test.GetType();

            //UnityConfigurationSection unityConfigurationSection = (UnityConfigurationSection)configuration.GetSection("unity");

            var loader = new ConfigFileLoader<ContainerFactory>("unity");
            UnityConfigurationSection unityConfigurationSection = loader.GetSection<UnityConfigurationSection>("unity");

            //载入名称为FirstClass 的container节点
            unityContainer.LoadConfiguration(unityConfigurationSection, "FirstClass");

            #region Test 1
            
            ////解析默认对象
            //IClass cbClass = unityContainer.Resolve<IClass>();
            //cbClass.ShowInfo();

            //指定命名解析对象
            IClass ecClass = unityContainer.Resolve<IClass>("ec");
            ecClass.ShowInfo();

            Console.WriteLine(ecClass.GetHashCode());

            ecClass = null;

            IClass ecClass1 = unityContainer.Resolve<IClass>("ec");
            ecClass1.ShowInfo();

            //Assert.IsTrue(ecClass1.Equals(ecClass));

            Console.WriteLine(ecClass1.GetHashCode());

            ecClass1 = null;

            //Assert.IsTrue(ecClass1.GetHashCode() == ecClass.GetHashCode());

            //获取容器中所有IClass的注册的已命名对象
            IEnumerable<IClass> classList = unityContainer.ResolveAll<IClass>();
            foreach (var item in classList)
            {
                item.ShowInfo();
            }
            
            #endregion

            ArrayDependencyObject<int> arrayDependencyObject = unityContainer.Resolve<ArrayDependencyObject<int>>("hfiuyuei");

            //arrayDependencyObject.Loggers.Select(l => l.GetType()).AssertContainsInAnyOrder(
            //    typeof(CbClass), typeof(EcClass), typeof(CbClass));

            //Assert.IsNotNull(arrayDependencyObject.Loggers);

            ////Assert.IsNotNull(unityContainer.Configure<MockContainerExtension>());

            //arrayDependencyObject.Stuff.AssertContainsExactly();

            //////unityContainer.RegisterType(typeof(GenericObjectWithConstructorDependency<>), "manual",
            //////    new InjectionConstructor(new GenericParameter("T")));
            //////var manualResult = unityContainer.Resolve<GenericObjectWithConstructorDependency<string>>("manual");

            ////var resultForString = unityContainer.Resolve<GenericObjectWithConstructorDependency<string>>("basic");
            ////Assert.AreEqual(unityContainer.Resolve<string>("third"), resultForString.Value);

            //var resultForInt = unityContainer.Resolve<GenericObjectWithConstructorDependency<int>>("basic");
            //Assert.AreEqual(unityContainer.Resolve<int>(), resultForInt.Value);

            ObjectWithOverloads objectWithOverloads = unityContainer.Resolve<ObjectWithOverloads>("callFirstOverloadTwice");

    */
            #endregion

            #region unity.generic.config


            var loader = new ConfigFileLoader<TResourceLocator>(unityConfigFileName);
            UnityConfigurationSection unityConfigurationSection = loader.GetSection<UnityConfigurationSection>(unitySectionName);

            //载入名称为FirstClass 的container节点
            unityContainer.LoadConfiguration(unityConfigurationSection, unityContainerName);

            //var hh = unityContainer.Resolve<ItemsCollection<IItem>>("ThroughConstructorWithSpecificElements");
            #endregion

            //Console.ReadLine();

            return unityContainer;
        }
    }
}

namespace Microsoft.Practices.Unity.Configuration.Tests.TestObjects
{
    using UnityContainerr;

    public class ArrayDependencyObject<T>
    {
        public IClass[] Loggers { get; set; }

        public T[] Stuff { get; set; }
        public string ConnectionString { get; private set; }
        public IClass Class { get; private set; }

        public void Initialize(string connectionString, IClass @class)
        {
            ConnectionString = connectionString;
            Class = @class;
        }

    }

    internal class GenericObjectWithConstructorDependency<T>
    {
        public T Value { get; private set; }

        public GenericObjectWithConstructorDependency(T value)
        {
            Value = value;
        }
    }

    internal class ObjectWithOverloads
    {
        public int FirstOverloadCalls;
        public int SecondOverloadCalls;

        public void CallMe(int param)
        {
            ++FirstOverloadCalls;
        }

        public void CallMe(string param)
        {
            ++SecondOverloadCalls;
        }
    }
}

namespace Microsoft.Practices.Unity.Configuration.Tests.TestObjects.MyGenericTypes
{
    public interface IItem
    {
        string ItemName { get; set; }
        string ItemType { get; }
        ItemCategory Category { get; }
    }

    public enum ItemCategory
    {
        Car,
        Van,
        Truck,
        Construction,
        Agricultural,
        Other
    }

    internal class DiggerItem : IItem
    {
        public DiggerItem(){ }

        public DiggerItem(string name, int bucketWidth)
        {
            this.ItemName = name;
            this.BucketWidthInches = bucketWidth;
        }

        #region IItem Members

        public string ItemName { get; set; }

        public string ItemType
        {
            get { return "Digger"; }
        }

        public ItemCategory Category
        {
            get { return ItemCategory.Construction; }
        }

        #endregion

        public int BucketWidthInches { get; set; }
    }

    public class ItemsCollection<T>
    {
        public IGenericService<T> Printer;
        public string CollectionName;

        public ItemsCollection(String name, IGenericService<T> printService)
        {
            CollectionName = name;
            Printer = printService;
        }

        public ItemsCollection(String name, IGenericService<T> printService, T[] items)
            : this(name, printService)
        {
            this.Items = items;
        }

        public ItemsCollection(String name, IGenericService<T> printService, T[][] itemsArray)
            : this(name, printService, itemsArray.Length > 0 ? itemsArray[0] : null)
        { }

        public T[] Items { get; set; }

        public object Item { get; set; }
    }

    public interface IGenericService<T>
    {
        string ServiceStatus { get; }
    }

    public class MyPrintService<T> : IGenericService<T>
    {
        private string status;
        #region IMyService Members

        public MyPrintService(string status) { this.status = status; }

        public string ServiceStatus
        {
            get { return String.Format("Available for type: {0}", this.status/*typeof(T)*/); }
        }

        #endregion
    }
}

