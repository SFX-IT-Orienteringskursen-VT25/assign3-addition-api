export class AdditionService {
  constructor() {
    this.history = [];
    this.sum = 0;
    this.initialized = false;
    this.baseUrl = window.location.origin;
  }

  // Initialize by fetching data from API
  async init() {
    if (this.initialized) return;
    
    try {
      const response = await fetch(`${this.baseUrl}/api/addition-data`);
      
      if (response.status === 200) {
        const result = await response.json();
        this.history = result.data?.history || [];
        this.sum = result.data?.sum || 0;
      } else if (response.status === 404) {
        // No existing data, start fresh
        this.history = [];
        this.sum = 0;
      } else {
        const errorData = await response.json();
        throw new Error(errorData.error || `Failed to fetch data: ${response.status}`);
      }
    } catch (error) {
      console.error('Failed to initialize AdditionService:', error);
  
      this.history = [];
      this.sum = 0;
    }
    
    this.initialized = true;
  }

  async add(num) {
  
    if (!this.initialized) {
      await this.init();
    }

    if (!Number.isInteger(num)) {
      throw new Error('Only integers are allowed');
    }
    
    this.history.push(num);
    this.sum += num;
    await this._save();
  }

  async getSum() {
    if (!this.initialized) {
      await this.init();
    }
    return this.sum;
  }

  async getHistory() {
    if (!this.initialized) {
      await this.init();
    }
    return [...this.history];
  }

  async _save() {
    try {
      const data = {
        history: this.history,
        sum: this.sum
      };

      const response = await fetch(`${this.baseUrl}/api/addition-data`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || `Failed to save data: ${response.status}`);
      }
    } catch (error) {
      console.error('Failed to save addition data:', error);
      throw error;
    }
  }
}