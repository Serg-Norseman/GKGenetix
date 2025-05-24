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

        void ShowAdmixture(string kit);
        void ShowPhasedSegmentVisualizer(string kit1, string kit2, string chr, string start_pos, string end_pos);
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
