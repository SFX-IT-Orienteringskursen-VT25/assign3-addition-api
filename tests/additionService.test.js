/**
 * @jest-environment jsdom
 */

import { AdditionService } from '../additionService.js';

global.fetch = jest.fn();

describe('AdditionService API Tests', () => {
  let service;

  beforeEach(() => {
    service = new AdditionService();
    fetch.mockClear();
  });

  afterEach(() => {
    jest.resetAllMocks();
  });

  describe('Initialization', () => {
    test('initializes with empty data when API returns 404', async () => {
    
      fetch.mockResolvedValueOnce({
        status: 404,
        json: () => Promise.resolve({ data: null, message: 'No addition data found' })
      });

      await service.init();
      
      expect(service.initialized).toBe(true);
      const sum = await service.getSum();
      const history = await service.getHistory();
      
      expect(sum).toBe(0);
      expect(history).toEqual([]);
    });

    test('initializes with existing data when API returns data', async () => {
      const mockData = { history: [5, 10], sum: 15 };
      
      fetch.mockResolvedValueOnce({
        status: 200,
        json: () => Promise.resolve({ success: true, data: mockData })
      });

      await service.init();
      
      const sum = await service.getSum();
      const history = await service.getHistory();
      
      expect(sum).toBe(15);
      expect(history).toEqual([5, 10]);
    });

    test('handles API errors gracefully during initialization', async () => {
      // Mock network error
      fetch.mockRejectedValueOnce(new Error('Network error'));

      await service.init();
      
      const sum = await service.getSum();
      const history = await service.getHistory();
      
      expect(sum).toBe(0);
      expect(history).toEqual([]);
    });
  });

  describe('Adding numbers', () => {
    test('adds valid integers and calls save API', async () => {
      // Mock initialization 
      fetch
        .mockResolvedValueOnce({
          status: 404,
          json: () => Promise.resolve({ data: null })
        })
        // Mock save API call
        .mockResolvedValueOnce({
          ok: true,
          status: 200,
          json: () => Promise.resolve({ 
            success: true, 
            message: 'Addition data saved successfully',
            data: { history: [5], sum: 5 }
          })
        });

      await service.add(5);
      
      expect(fetch).toHaveBeenCalledTimes(2);
      
      // Check the save API call
      const saveCall = fetch.mock.calls[1];
      expect(saveCall[0]).toContain('/api/addition-data');
      expect(saveCall[1].method).toBe('PUT');
      expect(saveCall[1].headers['Content-Type']).toBe('application/json');
      
      const sentData = JSON.parse(saveCall[1].body);
      expect(sentData).toEqual({ history: [5], sum: 5 });
    });

    test('throws error for non-integer input', async () => {
      // Mock initialization
      fetch.mockResolvedValueOnce({
        status: 404,
        json: () => Promise.resolve({ data: null })
      });

      await expect(service.add(5.5)).rejects.toThrow('Only integers are allowed');
      await expect(service.add('5')).rejects.toThrow('Only integers are allowed');
      await expect(service.add(null)).rejects.toThrow('Only integers are allowed');
    });

    test('handles save API errors', async () => {
    
      fetch
        .mockResolvedValueOnce({
          status: 404,
          json: () => Promise.resolve({ data: null })
        })
 
        .mockResolvedValueOnce({
          ok: false,
          status: 500,
          json: () => Promise.resolve({ error: 'Server error' })
        });

      await expect(service.add(5)).rejects.toThrow('Server error');
    });
  });

  describe('Getting data', () => {
    test('getSum returns current sum', async () => {
   
      fetch.mockResolvedValueOnce({
        status: 200,
        json: () => Promise.resolve({ 
          success: true, 
          data: { history: [1, 2, 3], sum: 6 }
        })
      });

      const sum = await service.getSum();
      expect(sum).toBe(6);
    });

    test('getHistory returns copy of history array', async () => {
     
      fetch.mockResolvedValueOnce({
        status: 200,
        json: () => Promise.resolve({ 
          success: true, 
          data: { history: [1, 2, 3], sum: 6 }
        })
      });

      const history = await service.getHistory();
      expect(history).toEqual([1, 2, 3]);
      
      history.push(999);
      const history2 = await service.getHistory();
      expect(history2).toEqual([1, 2, 3]);
    });
  });

  describe('Integration test', () => {
    test('full workflow: initialize, add numbers, persist', async () => {
      // Mock initialization 
      fetch
        .mockResolvedValueOnce({
          status: 404,
          json: () => Promise.resolve({ data: null })
        })
    
        .mockResolvedValueOnce({
          ok: true,
          status: 200,
          json: () => Promise.resolve({ success: true })
        })
    
        .mockResolvedValueOnce({
          ok: true,
          status: 200,
          json: () => Promise.resolve({ success: true })
        });

      await service.add(10);
      await service.add(5);

      const sum = await service.getSum();
      const history = await service.getHistory();

      expect(sum).toBe(15);
      expect(history).toEqual([10, 5]);
      expect(fetch).toHaveBeenCalledTimes(3); 
    });
  });
});