using NeoCortexApi;
using NeoCortexApi.Classifiers;
using NeoCortexApi.Entities;
using NeoCortexApi.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NeoCortexApi.Classifiers.HtmClassifier<string, NeoCortexApi.Entities.ComputeCycle>;

namespace HTMVideoLearning
{
    public class BetterLearning
    {
        /// <summary>
        /// Create Experiment Better Learning
        /// original: VideoLearning
        /// </summary>
        /// <param name="vidCfg"></param>
        /// <param name="htm"></param>
        public BetterLearning(VideoConfig vidCfg, HtmConfig htm)
        {
            #region Reading Dataset

            #endregion

            #region Learning using SpatialPooler with HomeostaticPlasticityController
            StagedLearning sl = new StagedLearning(htm);
            #endregion

            #region Learning after added Temporal Memory 

            #endregion
        }

        #region Prepare Training Environment

        public class StagedLearning {
            Connections mem;
            CortexLayer<object, object> layer = new("FirstLayer");
            private bool learn;
            private bool isInStableState;
            HtmClassifier<string, ComputeCycle> cls;
            public StagedLearning(HtmConfig htm, int maxNumOfElementsInSequence = 100)
            {
                mem = new(htm);
                learn = true;
                isInStableState = false;
                cls = new();
                HomeostaticPlasticityController hpa = new(mem, maxNumOfElementsInSequence * 150 * 3, (isStable, numPatterns, actColAvg, seenInputs) =>
                {
                    // We are not learning in instable state.
                    learn = isInStableState = isStable;

                    // Clear all learned patterns in the classifier.
                    cls.ClearState();

                }, numOfCyclesToWaitOnChange: 50);

                SpatialPoolerMT sp = new(hpa);
                sp.Init(mem);
                layer.HtmModules.Add("sp", sp);
            }
            public void activateTM()
            {
                TemporalMemory tm = new();
                tm.Init(mem);
                layer.AddModule("tm", tm);
            }
            public object Compute(object input, bool learn)
            {
                return layer.Compute(input, learn);
            }
            public void Learn(string key, Cell[] cells)
            {
                cls.Learn(key, cells);
            }
            public List<ClassifierResult> GetPredictedInputs(Cell[] predictiveCells, short howMany)
            {
                return cls.GetPredictedInputValues(predictiveCells, howMany);
            }
        }
        #endregion
    }
}