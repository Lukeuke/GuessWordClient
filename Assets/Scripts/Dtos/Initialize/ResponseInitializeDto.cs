using System;

namespace Dtos.Initialize
{
    [System.Serializable]
    public class ResponseInitializeDto
    {
        public string id;
        public int wonCount;
        public int totalMoneyWon;
        public bool canPlay;
    }
}