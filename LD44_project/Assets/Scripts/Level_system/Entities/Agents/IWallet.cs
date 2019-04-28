using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IWallet
{
    uint Coins { get; set; }
    void AddCoins();
    void SubCoins();
}
