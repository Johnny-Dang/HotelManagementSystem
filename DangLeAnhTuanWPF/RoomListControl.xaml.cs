using BLL.Services;
using DAL.Entities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DangLeAnhTuanWPF
{
    public partial class RoomListControl : UserControl
    {
        private readonly IRoomService _roomService;
        public RoomListControl(IRoomService roomService)
        {
            InitializeComponent();
            // UI chỉ khởi tạo service, service tự khởi tạo repository
            _roomService = roomService;
            var rooms = _roomService.GetAllRooms();
            dgRooms.ItemsSource = new ObservableCollection<RoomInformation>(rooms);
        }
    }
} 