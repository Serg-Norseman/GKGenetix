/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

namespace GenetixKit.Core
{
    public enum UIOperation
    {
        OPEN_KIT = 0,
        SELECT_ONE_TO_MANY,
        SELECT_ROH,
        SELECT_KIT,
        SELECT_MTPHYLOGENY,
        SELECT_MITOMAP,
        SELECT_ISOGGYTREE,
        SELECT_ADMIXTURE,
    }


    internal interface IKitHost
    {
        void SetStatus(string message);
        void SetProgress(int percent);
        void EnableSave();
        void DisableSave();
        void EnableToolbar();
        void DisableToolbar();
        void EnableDelete();
        void DisableDelete();

        void ShowAdmixture(string kit);
        void ShowPhasedSegmentVisualizer(string kit1, string kit2, string chr, int startPos, int endPos);
        void ShowProcessKits();
        void ShowQuickEdit();
        void ShowMatchingKits(string kit);
        void ShowROH(string kit);
        void ShowMtPhylogeny(string kit);
        void ShowMitoMap(string kit);
        void ShowIsoggYTree(string kit);
        void ShowOneToOneCmp(string kit1, string kit2);
        void SelectOper(UIOperation operation);
        void SelectLocation(ref int x, ref int y);

        void NewKit(string kit, bool disabled);
        string SelectKit();
        void DeleteKit();
    }
}
