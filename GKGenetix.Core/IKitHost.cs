/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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

        void NewKit();
        void OpenKit(string kit, bool disabled);
        void DeleteKit();
        void ImportTest();

        void ChangeKits(IList<TestRecord> selectedKits);

        void ShowPhasedSegmentVisualizer(string kit1, string kit2, byte chr, int startPos, int endPos);

        string SelectKit(char sex);
        void SelectLocation(TestRecord testRecord);

        void Exit();
        void ShowMessage(string msg);
        bool ShowQuestion(string msg);

        void SetAppDataPath(string path);
        void SetTestProvider(ITestProvider testProvider);
    }
}
