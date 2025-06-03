/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using GKGenetix.Core.Model;

namespace GGKit.Core
{
    public interface IKitHost
    {
        void SetStatus(string message);
        void SetProgress(int percent);
        void EnableSave();
        void DisableSave();
        void EnableToolbar();
        void DisableToolbar();
        void EnableDelete();
        void DisableDelete();
        void EnableExplore();
        void DisableExplore();

        void ShowAdmixture(IList<KitDTO> selectedKits);
        void ShowPhasedSegmentVisualizer(string kit1, string kit2, byte chr, int startPos, int endPos);
        void ShowProcessKits();
        void ShowMatchingKits(IList<KitDTO> selectedKits);
        void ShowROH(IList<KitDTO> selectedKits);
        void ShowMtPhylogeny(IList<KitDTO> selectedKits);
        void ShowMitoMap(IList<KitDTO> selectedKits);
        void ShowIsoggYTree(IList<KitDTO> selectedKits);
        void ShowOneToOneCmp(IList<KitDTO> selectedKits);
        void SelectLocation(ref int lng, ref int lat);
        void ShowMessage(string msg);
        void Exit();
        bool ShowQuestion(string msg);

        void NewKit();
        void OpenKit(string kit, bool disabled);
        string SelectKit(char sex);
        void DeleteKit();
        void ImportTest();

        void ChangeKits(IList<KitDTO> selectedKits);
    }
}
