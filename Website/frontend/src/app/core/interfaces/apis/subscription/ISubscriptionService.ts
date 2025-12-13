import type { ResultDto } from "../../dtos/ResultDto";
import type SubscriptionPlan from "./dtos/SubscriptionPlan";
import type SubscribeDto from "./dtos/SubscribeDto";

export default interface ISubscriptionService {
  getPlans(activeOnly?: boolean): Promise<SubscriptionPlan[]>;
  subscribe(subscribeDto: SubscribeDto): Promise<ResultDto>;
}

