/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using GKGenetix.Core.Model;

namespace GenetixKit.Core
{
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

        void ShowAdmixture(IList<KitDTO> selectedKits);
        void ShowPhasedSegmentVisualizer(string kit1, string kit2, string chr, int startPos, int endPos);
        void ShowProcessKits();
        void ShowMatchingKits(IList<KitDTO> selectedKits);
        void ShowROH(IList<KitDTO> selectedKits);
        void ShowMtPhylogeny(IList<KitDTO> selectedKits);
        void ShowMitoMap(IList<KitDTO> selectedKits);
        void ShowIsoggYTree(IList<KitDTO> selectedKits);
        void ShowOneToOneCmp(IList<KitDTO> selectedKits);
        void SelectLocation(ref int x, ref int y);

        void NewKit();
        void OpenKit(string kit, bool disabled);
        string SelectKit();
        void DeleteKit();
    }
}
