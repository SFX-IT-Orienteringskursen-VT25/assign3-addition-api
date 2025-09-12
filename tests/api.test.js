import request from 'supertest';
import app from '../server.js';
import { promises as fs } from 'fs';
import { join, dirname } from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const TEST_DATA_FILE = join(__dirname, '..', 'data', 'addition-data.json');

// Clean up test data before and after tests
beforeEach(async () => {
  try {
    await fs.unlink(TEST_DATA_FILE);
  } catch (error) {

  }
});

afterEach(async () => {
  try {
    await fs.unlink(TEST_DATA_FILE);
  } catch (error) {
  
  }
});

describe('Addition API Endpoints', () => {
  
  describe('GET /api/addition-data', () => {
    test('returns 404 when no data exists', async () => {
      const response = await request(app)
        .get('/api/addition-data')
        .expect(404);
      
      expect(response.body).toEqual({
        data: null,
        message: 'No addition data found'
      });
    });

    test('returns stored data when it exists', async () => {
      // First, save some data
      const testData = { history: [5, 10], sum: 15 };
      await request(app)
        .put('/api/addition-data')
        .send(testData)
        .expect(200);

      // Then retrieve it
      const response = await request(app)
        .get('/api/addition-data')
        .expect(200);
      
      expect(response.body).toEqual({
        success: true,
        data: testData
      });
    });
  });

  describe('PUT /api/addition-data', () => {
    test('saves valid data successfully', async () => {
      const testData = { history: [1, 2, 3], sum: 6 };
      
      const response = await request(app)
        .put('/api/addition-data')
        .send(testData)
        .expect(200);
      
      expect(response.body).toEqual({
        success: true,
        message: 'Addition data saved successfully',
        data: testData
      });
    });

    test('validates data format', async () => {
      const invalidData = { history: 'not-an-array', sum: 10 };
      
      const response = await request(app)
        .put('/api/addition-data')
        .send(invalidData)
        .expect(400);
      
      expect(response.body.error).toBe('Invalid data format. Expected history array and sum number.');
    });

    test('validates that history contains only integers', async () => {
      const invalidData = { history: [1, 2.5, 3], sum: 6.5 };
      
      const response = await request(app)
        .put('/api/addition-data')
        .send(invalidData)
        .expect(400);
      
      expect(response.body.error).toBe('All history items must be integers.');
    });

    test('validates that sum matches history', async () => {
      const invalidData = { history: [1, 2, 3], sum: 10 }; // Should be 6
      
      const response = await request(app)
        .put('/api/addition-data')
        .send(invalidData)
        .expect(400);
      
      expect(response.body.error).toBe('Sum does not match history calculation.');
    });

    test('handles empty history correctly', async () => {
      const testData = { history: [], sum: 0 };
      
      const response = await request(app)
        .put('/api/addition-data')
        .send(testData)
        .expect(200);
      
      expect(response.body.data).toEqual(testData);
    });
  });

  describe('Integration test', () => {
    test('can save and retrieve data', async () => {
      const originalData = { history: [10, 20, -5], sum: 25 };
      
      // Save data
      await request(app)
        .put('/api/addition-data')
        .send(originalData)
        .expect(200);
      
      // Retrieve data
      const response = await request(app)
        .get('/api/addition-data')
        .expect(200);
      
      expect(response.body.data).toEqual(originalData);
    });
  });
});