/*
 *  "GKGenetix", the simple DNA analysis kit.
 *  Copyright (C) 2022-2025 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GKGenetix".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using GKGenetix.Core.Database;

namespace GKGenetix.Core
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

        void ShowAdmixture(IList<TestRecord> selectedKits);
        void ShowPhasedSegmentVisualizer(string kit1, string kit2, byte chr, int startPos, int endPos);
        void ShowProcessKits();
        void ShowMatchingKits(IList<TestRecord> selectedKits);
        void ShowROH(IList<TestRecord> selectedKits);
        void ShowMtPhylogeny(IList<TestRecord> selectedKits);
        void ShowMitoMap(IList<TestRecord> selectedKits);
        void ShowIsoggYTree(IList<TestRecord> selectedKits);
        void ShowOneToOneCmp(IList<TestRecord> selectedKits);
        void SelectLocation(ref int lng, ref int lat);
        void ShowMessage(string msg);
        void Exit();
        bool ShowQuestion(string msg);

        void NewKit();
        void OpenKit(string kit, bool disabled);
        string SelectKit(char sex);
        void DeleteKit();
        void ImportTest();

        void ChangeKits(IList<TestRecord> selectedKits);
    }
}
