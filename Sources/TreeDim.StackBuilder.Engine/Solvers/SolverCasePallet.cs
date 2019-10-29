﻿using System.Collections.Generic;

using Sharp3D.Math.Core;
using log4net;

using treeDiM.StackBuilder.Basics;

namespace treeDiM.StackBuilder.Engine
{
    public class SolverCasePallet : ISolver
    {
        public SolverCasePallet(PackableBrick packable, PalletProperties palletProperties)
        {
            _packable = packable;
            _palletProperties = palletProperties;
        }

        public Layer2DBrickImp BuildBestLayer(ConstraintSetAbstract constraintSet)
        {
            var constraintSetCasePallet = constraintSet as ConstraintSetCasePallet;
            Vector2D overhang = constraintSetCasePallet.Overhang;
            // build layer list
            var solver = new LayerSolver();
            List<Layer2DBrickImp> layers = solver.BuildLayers(
                _packable.OuterDimensions
                , new Vector2D(_palletProperties.Length + 2.0 * overhang.X, _palletProperties.Width + 2.0 * overhang.Y)
                , _palletProperties.Height
                , constraintSetCasePallet
                , true
                );
            if (layers.Count > 0)
                return layers[0];
            return null;
        }

        public List<AnalysisHomo> BuildAnalyses(ConstraintSetAbstract constraintSet, bool allowMultipleLayerOrientations)
        {
            var analyses = new List<AnalysisHomo>();
            var constraintSetCasePallet = constraintSet as ConstraintSetCasePallet;
            if (null == constraintSetCasePallet)
                return analyses;
            Vector2D overhang = constraintSetCasePallet.Overhang;

            if (allowMultipleLayerOrientations)
            {
                var listLayerEncap = new List<KeyValuePair<LayerEncap, int>>();
                LayerSolver.GetBestCombination(
                    _packable.OuterDimensions,
                    _palletProperties.GetStackingDimensions(constraintSet),
                    constraintSet,
                    ref listLayerEncap);
                Solution.SetSolver(new LayerSolver());
                var analysis = new AnalysisCasePallet(_packable, _palletProperties, constraintSet as ConstraintSetCasePallet);
                analysis.AddSolution(listLayerEncap);
                // only add analysis if it has a valid solution
                if (analysis.Solution.ItemCount > 0)
                    analyses.Add(analysis);
            }
            else
            {
                // build layer list
                var solver = new LayerSolver();
                List<Layer2DBrickImp> layers = solver.BuildLayers(
                    _packable.OuterDimensions
                    , new Vector2D(_palletProperties.Length + 2.0 * overhang.X, _palletProperties.Width + 2.0 * overhang.Y)
                    , _palletProperties.Height
                    , constraintSetCasePallet
                    , true
                    );
                Solution.SetSolver(solver);
                // loop on layers
                foreach (Layer2DBrickImp layer in layers)
                {
                    var layerDescs = new List<LayerDesc> { layer.LayerDescriptor };
                    var analysis = new AnalysisCasePallet(_packable, _palletProperties, constraintSet as ConstraintSetCasePallet);
                    analysis.AddSolution(layerDescs);
                    // only add analysis if it has a valid solution
                    if (analysis.Solution.ItemCount > 0)
                        analyses.Add(analysis);
                }
            }
            return analyses;
        }

        #region Non-Public Members
        private PackableBrick _packable;
        private PalletProperties _palletProperties;
        static readonly ILog _log = LogManager.GetLogger(typeof(SolverCasePallet));
        #endregion
    }
}
