import { v1 as uuidv1, v4 as uuidv4 } from 'uuid';

async function GetBankIDs() {
    const response = fetch("/api/banks", {
        method: "GET",
        headers: { "Accept": "aplication/json" }
    });
    if (response.ok === true) {
        const banks = await response.json();
        let rows = document.getElementById('bank-id-selector');
        banks.forEach(bank => {
            rows.append(bank);
        });
    }
}
async function GetBankID(id){
    const response = await fetch("api/banks" + id, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true){
        return response;
    }
}

async function CreateBankId(userID, bankID){
    await fetch("api/banks", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            ID: uuidv4(),
            BankID: bankID,
            UserBankAccountID: userID,
            BankAccountAmount: 0
        })
    });
}