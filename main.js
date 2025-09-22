const readline = require('readline');
const { persistAndCalculate } = require('./persistence');

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

async function main() {
    console.log('Persisted Addition System (JavaScript)');
    console.log('Enter numbers one by one (type "done" to finish):');
    
    const numbers = [];
    
    const askNumber = () => {
        return new Promise((resolve) => {
            rl.question('Enter a number (or "done" to finish): ', (answer) => {
                resolve(answer);
            });
        });
    };
    
    while (true) {
        const input = await askNumber();
        
        if (input.toLowerCase() === 'done') {
            break;
        }
        
        const number = parseFloat(input);
        if (isNaN(number)) {
            console.log('Please enter a valid number or "done" to finish');
        } else {
            numbers.push(number);
            console.log(`Added: ${number}`);
        }
    }
    
    const total = persistAndCalculate(numbers);
    console.log(`\nNumbers added: ${numbers.join(', ')}`);
    console.log(`Total sum of all persisted numbers: ${total}`);
    
    rl.close();
}

main().catch(console.error);