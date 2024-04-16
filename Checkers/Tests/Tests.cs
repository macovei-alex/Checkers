using Checkers.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Checkers.Models;
using Checkers.Utilities;
using Checkers.ViewModels;

namespace Checkers.Tests
{
	public class Tests : BaseTest
	{
		private FileManager FileManager { get; set; }

		public Tests()
		{
			FileManager = new FileManager(FileManager.DefaultSavesFolderPath);
		}

		public override void RunTests()
		{
			// Test1();
			// Test2();
			// Test3();
		}

		private void Test1()
		{
			Game game = new Game(new Board());
			FileManager.SaveGame("test1.json", game, true);
			Game copy = FileManager.LoadGame("test1.json", true);

			AssertEqual(game, copy);
			FileManager.DeleteGame("test1.json");
		}

		private void Test2()
		{
			Game game = new Game(new Board());
			FileManager.SaveGame("test1.json", game, true);
			Game copy = FileManager.LoadGame("test1.json", true);

			AssertThrows<GameException>(() => game.Move(new Pair(2, 1)));
			AssertThrows<GameException>(() => game.Move(new Pair(2, 1), null));
			AssertThrows<GameException>(() => game.Move(new Pair(2, 1), new Pair[1] { null }));
			AssertThrows<GameException>(() => game.Move(new Pair(2, 1), new Pair(2, 2)));
			AssertThrows<GameException>(() => game.Move(new Pair(2, 1), new Pair(3, 1)));
			AssertThrows<GameException>(() => game.Move(new Pair(2, 1), new Pair(1, 2)));
			AssertThrows<GameException>(() => game.Move(new Pair(2, 1), new Pair(4, 3)));
			AssertThrows<GameException>(() => game.Move(new Pair(2, 1), new Pair(3, 2), new Pair(2, 1)));

			game.Move(new Pair(2, 1), new Pair(3, 2));

			AssertNotEqual(game, copy);

			game.Move(new Pair(5, 2), new Pair(4, 1));
			game.Move(new Pair(3, 2), new Pair(4, 3));
			game.Move(new Pair(5, 4), new Pair(3, 2));

			AssertEqual(game, copy);
		}

		private void Test3()
		{

			Game game = new Game(new Board());
			FileManager.SaveGame("test1.json", game, true);
			Game copy = FileManager.LoadGame("test1.json", true);

			AssertThrows<GameException>(() => game.Move(2, 1));
			AssertThrows<GameException>(() => game.Move(2, 1, null));
			AssertThrows<GameException>(() => game.Move(2, 1, 2, 2));
			AssertThrows<GameException>(() => game.Move(2, 1, 3, 1));
			AssertThrows<GameException>(() => game.Move(2, 1, 1, 2));
			AssertThrows<GameException>(() => game.Move(2, 1, 4, 3));
			AssertThrows<GameException>(() => game.Move(2, 1, 3, 2, 2, 1));

			game.Move(2, 1, 3, 2);
			AssertNotEqual(game, copy);

			game.Move(5, 2, 4, 1);
			game.Move(3, 2, 4, 3);
			game.Move(5, 4, 3, 2);

			AssertEqual(game, copy);
		}
	}
}
