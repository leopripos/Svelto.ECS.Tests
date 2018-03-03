﻿using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Svelto.ECS;

namespace UnitTests
{
    [TestClass]
    public class TestAddAndRemove
    {
        [TestInitialize]
        public void Init()
        {
            _simpleSubmissionEntityViewScheduler = new SimpleSubmissionEntityViewScheduler();
            _enginesRoot = new EnginesRoot(_simpleSubmissionEntityViewScheduler);
            _neverDoThisIsJustForTheTest = new TestEngine();
            _enginesRoot.AddEngine(_neverDoThisIsJustForTheTest);
            _entityFactory = _enginesRoot.GenerateEntityFactory();
            _entityFunctions = _enginesRoot.GenerateEntityFunctions();
        }

        [TestMethod]
        [ExpectedException(typeof(TypeSafeDictionaryException))]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestExceptionThrownOnDoubleAddIntervalled(int id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFactory.BuildEntity<TestDescriptor>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        [TestMethod]
        [ExpectedException(typeof(TypeSafeDictionaryException))]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestExceptionThrownOnDoubleAdd(int id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(id, null);
            _entityFactory.BuildEntity<TestDescriptor>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        [TestMethod]
        [ExpectedException(typeof(TypeSafeDictionaryException))]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestExceptionThrownOnDoubleEntityViewAddIntervalled(int id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFactory.BuildEntity<TestDescriptor2>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        [TestMethod]
        [ExpectedException(typeof(TypeSafeDictionaryException))]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestExceptionThrownOnDoubleEntityViewAdd(int id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(id, null);
            _entityFactory.BuildEntity<TestDescriptor2>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        [TestMethod]
        [ExpectedException(typeof(TypeSafeDictionaryException))]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestTwoEntitiesWithSameIDThrowsIntervalled(int id)
        {
            _entityFactory.BuildEntity<TestDescriptor2>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFactory.BuildEntity<TestDescriptor3>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        [TestMethod]
        [ExpectedException(typeof(TypeSafeDictionaryException))]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestTwoEntitiesWithSameIDThrows(int id)
        {
            _entityFactory.BuildEntity<TestDescriptor2>(id, null);
            _entityFactory.BuildEntity<TestDescriptor3>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestMetaEntityAndEntityIsOkWithSameID(int id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(id, null);
            _entityFactory.BuildMetaEntity<TestDescriptor>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _neverDoThisIsJustForTheTest.HasEntity<TestEntityView>(id);
            _neverDoThisIsJustForTheTest.HasMetaEntity<TestEntityView>(id);
        }
        
        [TestMethod]
        [ExpectedException(typeof(TypeSafeDictionaryException))]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestTwoEntitiesWithSameIDRegardlessTheGroupThrows(int id)
        {
            _entityFactory.BuildEntityInGroup<TestDescriptor2>(id, id, null);
            _entityFactory.BuildEntityInGroup<TestDescriptor3>(id, id+1, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();
        }
        
        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestRemoveEntity(int id)
        {
            _entityFactory.BuildEntity<TestDescriptor>(id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFunctions.RemoveEntity(id);

            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityView>(id));
        }
        
        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestAddEntityToGroup(int id)
        {
            _entityFactory.BuildEntityInGroup<TestDescriptor>(id, id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityView>(id));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntitiesInGroup<TestEntityView>(id));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestRemoveEntityFromGroup(int id)
        {
            _entityFactory.BuildEntityInGroup<TestDescriptor>(id, id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFunctions.RemoveEntity(id);

            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityView>(id));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntitiesInGroup<TestEntityView>(id));
        }
        
        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestRemoveEntityGroup(int id)
        {
            _entityFactory.BuildEntityInGroup<TestDescriptor>(id, id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFunctions.RemoveGroupAndEntities(id);

            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntity<TestEntityView>(id));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntitiesInGroup<TestEntityView>(id));
        }
        
        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void TestRemoveSwapGroup(int id)
        {
            _entityFactory.BuildEntityInGroup<TestDescriptor>(id, id, null);
            _simpleSubmissionEntityViewScheduler.SubmitEntities();

            _entityFunctions.SwapEntityGroup(id, id, 3);

            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntity<TestEntityView>(id));
            Assert.IsFalse(_neverDoThisIsJustForTheTest.HasEntitiesInGroup<TestEntityView>(id));
            Assert.IsTrue(_neverDoThisIsJustForTheTest.HasEntitiesInGroup<TestEntityView>(3));
        }
        
        EnginesRoot                         _enginesRoot;
        IEntityFactory                      _entityFactory;
        IEntityFunctions                    _entityFunctions;
        SimpleSubmissionEntityViewScheduler _simpleSubmissionEntityViewScheduler;
        TestEngine                          _neverDoThisIsJustForTheTest;

        class TestDescriptor : GenericEntityDescriptor<TestEntityView>
        {}
        
        class TestDescriptor2 : GenericEntityDescriptor<TestEntityView>
        {}
        
        class TestDescriptor3 : GenericEntityDescriptor<TestEntityView2>
        {}

        class TestEntityView : EntityView
        {}
        
        class TestEntityView2 : EntityView
        {}

        class TestEngine: IQueryingEntityViewEngine
        {
            public IEntityViewsDB entityViewsDB { get; set; }
            public void Ready() {}

            public bool HasEntity<T>(int ID) where T : EntityView
            {
                T view;
                return entityViewsDB.TryQueryEntityView(ID, out view);
            }
            
            public bool HasEntitiesInGroup<T>(int ID) where T : EntityView
            {
                return entityViewsDB.QueryGroupedEntityViews<T>(ID).Count != 0;
            }

            public bool HasMetaEntity<T>(int ID) where T : EntityView
            {
                T view;
                return entityViewsDB.TryQueryMetaEntityView(ID, out view);
            }
        }
    }
}