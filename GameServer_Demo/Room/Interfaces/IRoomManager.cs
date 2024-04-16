using GameServer_Demo.Room.Handlers;


namespace GameServer_Demo.Room.Interfaces
{
    public interface IRoomManager
    {
        Lobby Lobby { get; set; }
        BaseRoom CreateRoom(int timer, string ownerId = "");
        BaseRoom FindRoom(string id);

        List<BaseRoom> ListRoom();
        bool RemoveRoom(string Id);
    }
}
