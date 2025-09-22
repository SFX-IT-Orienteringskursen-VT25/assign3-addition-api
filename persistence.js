const fs = require('fs');
const path = require('path');

// Default filename, can be overridden for testing
let dataFile = path.join(__dirname, 'numbers.json');

function persistAndCalculate(numbers = null) {
    let existingNumbers = [];
    
    // Read existing numbers if file exists
    try {
        if (fs.existsSync(dataFile)) {
            const data = fs.readFileSync(dataFile, 'utf8');
            existingNumbers = JSON.parse(data);
        }
    } catch (error) {
        console.error('Error reading file:', error);
        existingNumbers = [];
    }
    
    // Add new numbers if provided
    if (numbers && Array.isArray(numbers)) {
        existingNumbers = existingNumbers.concat(numbers);
        
        try {
            fs.writeFileSync(dataFile, JSON.stringify(existingNumbers, null, 2));
        } catch (error) {
            console.error('Error writing file:', error);
            throw error;
        }
    }
    
    // Calculate and return sum
    const sum = existingNumbers.reduce((total, num) => total + num, 0);
    return sum;
}

// Helper function for testing to set different file path
function setTestFile(filename) {
    dataFile = filename;
}

module.exports = { 
    persistAndCalculate,
    setTestFile // Export for testing
};