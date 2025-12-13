export default interface SubscriptionPlan {
  id: string;
  name: string;
  description: string;
  price: number;
  billingCycle: string;
  isFree: boolean;
  features: string; // JSON string
  isActive: boolean;
}

