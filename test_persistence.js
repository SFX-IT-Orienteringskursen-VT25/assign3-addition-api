const { persistAndCalculate, setTestFile } = require('./persistence');
const fs = require('fs');
const path = require('path');

describe('Persisted Addition Tests', () => {
    const testFile = path.join(__dirname, 'test-numbers.json');
    
    beforeEach(() => {
        // Set test file for this test
        setTestFile(testFile);
        // Clean up test file before each test
        if (fs.existsSync(testFile)) {
            fs.unlinkSync(testFile);
        }
    });
    
    afterEach(() => {
        // Clean up after each test
        if (fs.existsSync(testFile)) {
            fs.unlinkSync(testFile);
        }
    });
    
    test('should return 0 when no numbers exist', () => {
        const result = persistAndCalculate();
        expect(result).toBe(0);
    });
    
    test('should persist numbers and return correct sum', () => {
        const result = persistAndCalculate([1, 2, 3]);
        expect(result).toBe(6);
    });
    
    test('should add to existing numbers and return total sum', () => {
        persistAndCalculate([4, 5]);
        const result = persistAndCalculate([6]);
        expect(result).toBe(15);
    });
    
    test('should handle decimal numbers correctly', () => {
        const result = persistAndCalculate([1.5, 2.5, 3.2]);
        expect(result).toBeCloseTo(7.2);
    });
    
    test('should return sum without modifying when no new numbers provided', () => {
        persistAndCalculate([10, 20]);
        const result = persistAndCalculate();
        expect(result).toBe(30);
    });
});