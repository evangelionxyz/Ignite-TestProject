using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestProject.Scripts.Cards;
using TestProject.Scripts.Cards.Effects.EffectMisc;
using TestProject.Scripts.Cards.Misc;

namespace TestProject.Scripts
{
    public class CardRegistry
    {
        const string CARD_BASE_DATA_PATH = "Scripts/Data/Cards/";
        const string POLICY_PATH = "policies.json";
        const string ACTION_PATH = "actions.json";
        const string MANDATE_PATH = "mandates.json";
        const string PROSPECT_PATH = "prospects.json";
        const string CONTRACT_PATH = "contracts.json";

        private static Dictionary<string, PolicyCard> policyDictionary = [];
        private static Dictionary<string, ActionCard> actionDictionary = [];
        private static Dictionary<string, MandateCard> mandateDictionary = [];
        private static Dictionary<string, ProspectCard> prospectDictionary = [];
        private static Dictionary<string, DebtContract> debtDictionary = [];

        private static string GetCardId<T>(int numericId)
        {
            string prefix = typeof(T) switch
            {
                var t when t == typeof(PolicyCard) => "POL",
                var t when t == typeof(ActionCard) => "ACT",
                var t when t == typeof(MandateCard) => "MAN",
                var t when t == typeof(ProspectCard) => "PRO",
                var t when t == typeof(DebtContract) => "CON",
                _ => throw new ArgumentException($"Unknown card type: {typeof(T).Name}")
            };
            return $"{prefix}_{numericId}";
        }

        private static void LoadCardData()
        {
            policyDictionary = [];
            actionDictionary = [];
            mandateDictionary = [];
            prospectDictionary = [];
            debtDictionary = [];

            string basePath = CARD_BASE_DATA_PATH;

            try
            {
                LoadPolicyCards(Path.Combine(basePath, POLICY_PATH));
                LoadActionCards(Path.Combine(basePath, ACTION_PATH));
                LoadMandateCards(Path.Combine(basePath, MANDATE_PATH));
                LoadProspectCards(Path.Combine(basePath, PROSPECT_PATH));
                LoadDebtContracts(Path.Combine(basePath, CONTRACT_PATH));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading card data: {ex.Message}");
                throw;
            }
        }

        private static void LoadPolicyCards(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Warning: File does not exist: {path}");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cards = JsonSerializer.Deserialize<List<PolicyCardData>>(json, options);

                if (cards != null)
                {
                    foreach (var cardData in cards)
                    {
                        var card = cardData.ToCard();
                        if (card != null)
                        {
                            policyDictionary[cardData.Id] = card;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading policy cards from {path}: {ex.Message}");
            }
        }

        private static void LoadActionCards(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Warning: File does not exist: {path}");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cards = JsonSerializer.Deserialize<List<ActionCardData>>(json, options);

                if (cards != null)
                {
                    foreach (var cardData in cards)
                    {
                        var card = cardData.ToCard();
                        if (card != null)
                        {
                            actionDictionary[cardData.Id] = card;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading action cards from {path}: {ex.Message}");
            }
        }

        private static void LoadMandateCards(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Warning: File does not exist: {path}");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cards = JsonSerializer.Deserialize<List<MandateCardData>>(json, options);

                if (cards != null)
                {
                    foreach (var cardData in cards)
                    {
                        var card = cardData.ToCard();
                        if (card != null)
                        {
                            mandateDictionary[cardData.Id] = card;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading mandate cards from {path}: {ex.Message}");
            }
        }

        private static void LoadProspectCards(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Warning: File does not exist: {path}");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cards = JsonSerializer.Deserialize<List<ProspectCardData>>(json, options);

                if (cards != null)
                {
                    foreach (var cardData in cards)
                    {
                        var card = cardData.ToCard();
                        if (card != null)
                        {
                            prospectDictionary[cardData.Id] = card;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading prospect cards from {path}: {ex.Message}");
            }
        }

        private static void LoadDebtContracts(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Warning: File does not exist: {path}");
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var contracts = JsonSerializer.Deserialize<List<DebtContractData>>(json, options);

                if (contracts != null)
                {
                    foreach (var contractData in contracts)
                    {
                        var contract = contractData.ToContract();
                        if (contract != null)
                        {
                            debtDictionary[contractData.Id] = contract;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading debt contracts from {path}: {ex.Message}");
            }
        }

        public static T Get<T>(int id) where T : Card
        {
            string cardId = GetCardId<T>(id);
            return GetById<T>(cardId);
        }

        public static T GetById<T>(string id) where T : Card
        {
            if (typeof(T) == typeof(PolicyCard))
            {
                if (policyDictionary.Count == 0)
                    LoadCardData();
                if (policyDictionary!.TryGetValue(id, out var card))
                    return card as T;
            }
            else if (typeof(T) == typeof(ActionCard))
            {
                if (actionDictionary.Count == 0)
                    LoadCardData();
                if (actionDictionary!.TryGetValue(id, out var card))
                    return card as T;
            }
            else if (typeof(T) == typeof(MandateCard))
            {
                if (mandateDictionary.Count == 0)
                    LoadCardData();
                if (mandateDictionary!.TryGetValue(id, out var card))
                    return card as T;
            }
            else if (typeof(T) == typeof(ProspectCard))
            {
                if (prospectDictionary.Count == 0)
                    LoadCardData();
                if (prospectDictionary!.TryGetValue(id, out var card))
                    return card as T;
            }
            else if (typeof(T) == typeof(DebtContract))
            {
                if (debtDictionary.Count == 0)
                    LoadCardData();
                if (debtDictionary!.TryGetValue(id, out var card))
                    return card as T;
            }
            throw new KeyNotFoundException($"Card of type '{typeof(T).Name}' with ID '{id}' not found.");
        }

        public static List<T> GetAll<T>() where T : Card
        {
            if (typeof(T) == typeof(PolicyCard))
            {
                if (policyDictionary.Count == 0)
                    LoadCardData();
                return policyDictionary!.Values.Cast<T>().ToList();
            }
            else if (typeof(T) == typeof(ActionCard))
            {
                if (actionDictionary.Count == 0)
                    LoadCardData();
                return actionDictionary!.Values.Cast<T>().ToList();
            }
            else if (typeof(T) == typeof(MandateCard))
            {
                if (mandateDictionary.Count == 0)
                    LoadCardData();
                return mandateDictionary!.Values.Cast<T>().ToList();
            }
            else if (typeof(T) == typeof(ProspectCard))
            {
                if (prospectDictionary.Count == 0)
                    LoadCardData();
                return prospectDictionary!.Values.Cast<T>().ToList();
            }
            throw new KeyNotFoundException($"Cards of type '{typeof(T).Name}' not found.");
        }

        // JSON Deserialization Helper Classes
        private class PolicyCardData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string EffectDesc { get; set; }
            public int Type { get; set; }
            public int BaseGdp { get; set; }
            public int EffectToNature { get; set; }
            public int EffectToHighClass { get; set; }
            public int EffectToLowClass { get; set; }
            public List<object> Effects { get; set; } = [];

            public PolicyCard ToCard()
            {
                try
                {
                    // Create empty effects list for now
                    var effectsList = new List<IEffect>();

                    var card = new PolicyCard(
                        Id,
                        Name,
                        EffectDesc,
                        effectsList,
                        (EPolicyType)Type,
                        BaseGdp,
                        EffectToNature,
                        EffectToHighClass,
                        EffectToLowClass
                    );
                    return card;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting PolicyCardData: {ex.Message}");
                    return null;
                }
            }
        }

        private class ActionCardData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string EffectDesc { get; set; }
            public int SellPrice { get; set; }
            public List<object> Effects { get; set; } = [];

            public ActionCard ToCard()
            {
                try
                {
                    var effectsList = new List<IEffect>();

                    var card = new ActionCard(
                        Id,
                        Name,
                        EffectDesc,
                        effectsList,
                        SellPrice
                    );
                    return card;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting ActionCardData: {ex.Message}");
                    return null;
                }
            }
        }

        private class MandateCardData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string EffectDesc { get; set; }
            public int Rarity { get; set; }
            public bool IsSellable { get; set; }
            public int SellPrice { get; set; }
            public List<object> Effects { get; set; } = [];

            public MandateCard ToCard()
            {
                try
                {
                    var effectsList = new List<IEffect>();

                    var card = new MandateCard(
                        Id,
                        Name,
                        EffectDesc,
                        effectsList,
                        (ERarity)Rarity,
                        IsSellable,
                        SellPrice
                    );
                    return card;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting MandateCardData: {ex.Message}");
                    return null;
                }
            }
        }

        private class ProspectCardData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string EffectDesc { get; set; }
            public List<object> Effects { get; set; } = [];

            public ProspectCard ToCard()
            {
                try
                {
                    var effectsList = new List<IEffect>();

                    var card = new ProspectCard(
                        Id,
                        Name,
                        EffectDesc,
                        effectsList
                    );
                    return card;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting ProspectCardData: {ex.Message}");
                    return null;
                }
            }
        }

        private class DebtContractData
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int AmountToGet { get; set; }
            public int Countdown { get; set; }
            public string PenaltyDesc { get; set; }
            public string MandatesIdToChange { get; set; }

            public DebtContract ToContract()
            {
                try
                {
                    var contract = new DebtContract(
                        Id,
                        Name,
                        AmountToGet,
                        Countdown,
                        PenaltyDesc,
                        MandatesIdToChange
                    );
                    return contract;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting DebtContractData: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
