using System;
using System.Reflection;

namespace MapReduce.Net.Impl
{
    public class JobConfigurator : IJobConfigurator
    {
        public Type TypeOfMapper { get; private set; }
        public Type TypeOfCombiner { get; private set; }
        public Type TypeOfReducer { get; private set; }
        public Type TypeOfDataBatchProcessor { get; private set; }
        public int NumberOfMappersPerNode { get; private set; }
        public int NumberOfChunks { get; private set; }
        public IDependancyScope DependancyScope { get; private set; }

        public JobConfigurator()
        {
            NumberOfChunks = 4;
            NumberOfMappersPerNode = 0;
        }

        public JobConfigurator(Type typeOfMapper, Type typeOfCombiner, Type typeOfReducer, Type typeOfDataBatchProcessor, int numberOfMappersPerNode = 0, int numberOfChunks = 4, IDependancyScope dependancyScope = null)
        {
            if (numberOfChunks == 0)
            {
                numberOfChunks = 4;
            }
            TypeOfMapper = typeOfMapper;
            TypeOfCombiner = typeOfCombiner;
            TypeOfReducer = typeOfReducer;
            TypeOfDataBatchProcessor = typeOfDataBatchProcessor;
            NumberOfMappersPerNode = numberOfMappersPerNode;
            NumberOfChunks = numberOfChunks;
            DependancyScope = dependancyScope;
        }

        public JobConfigurator UseMapper(Type ofType)
        {
            TypeOfMapper = ofType;
            return this;
        }

        public JobConfigurator UseCombiner(Type ofType)
        {
            TypeOfCombiner = ofType;
            return this;
        }

        public JobConfigurator UseReducer(Type ofType)
        {
            TypeOfReducer = ofType;
            return this;
        }

        public JobConfigurator UseDataBatchProcessor(Type ofType)
        {
            TypeOfDataBatchProcessor = ofType;
            return this;
        }

        public JobConfigurator UseIoC(IDependancyScope scope)
        {
            DependancyScope = scope;
            return this;
        }

        public JobConfigurator WithNumberOfMapperPerNode(int number)
        {
            NumberOfMappersPerNode = number;
            return this;
        }

        public JobConfigurator WithNumberOfChunk(int number)
        {
            NumberOfChunks = number;
            return this;
        }

        public void ValidateConfiguration()
        {
            if (TypeOfMapper == null)
            {
                throw new ArgumentException($"{nameof(TypeOfMapper)} cannot be null ");
            }
            if (TypeOfReducer == null)
            {
                throw new ArgumentException($"{nameof(TypeOfReducer)} cannot be null ");
            }
            if (TypeOfDataBatchProcessor == null)
            {
                throw new ArgumentException($"{nameof(TypeOfDataBatchProcessor)} cannot be null ");
            }

            if (!typeof(IMapper).GetTypeInfo().IsAssignableFrom(TypeOfMapper.GetTypeInfo()))
            {
                throw new ArgumentException($"{nameof(IMapper)} is not assignable from {nameof(TypeOfMapper)} ");
            }

            if (!typeof(IReducer).GetTypeInfo().IsAssignableFrom(TypeOfReducer.GetTypeInfo()))
            {
                throw new ArgumentException($"{nameof(IReducer)} is not assignable from {nameof(TypeOfReducer)} ");
            }

            if (TypeOfCombiner != null)
            {
                if (!typeof(ICombiner).GetTypeInfo().IsAssignableFrom(TypeOfCombiner.GetTypeInfo()))
                {
                    throw new ArgumentException($"{nameof(ICombiner)} is not assignable from {nameof(TypeOfCombiner)} ");
                }
            }
        }
    }
}