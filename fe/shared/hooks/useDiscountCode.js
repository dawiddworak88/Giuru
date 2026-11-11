import { useCallback, useState } from "react";

export const useDiscountCode = ({ initialCode = "", initialCodeIsNotValid = false, isEnabled, onSave }) => {
    const seedCode = isEnabled ? (initialCode || "") : "";
    const [discountCode, setDiscountCode] = useState(seedCode);
    const [appliedDiscountCode, setAppliedDiscountCode] = useState(seedCode);

    // A stored code that stopped being valid is still seeded as applied, so that it can be removed:
    // checkout refuses the basket until it is. Any basket response settles the code, so it clears this.
    const [isAppliedDiscountCodeNotValid, setIsAppliedDiscountCodeNotValid] = useState(Boolean(seedCode) && initialCodeIsNotValid);

    // syncFromDiscountCode, syncFromResponse and reset only ever touch the setters, so they are
    // memoised with no dependencies and keep a stable identity. Callers put them in the dependency
    // arrays of their own useCallbacks (the dropzone handler, the order-management hook), and an
    // identity that changed on every render would silently defeat that memoisation.
    const syncFromDiscountCode = useCallback((savedDiscountCode) => {
        const normalizedDiscountCode = savedDiscountCode || "";

        setDiscountCode(normalizedDiscountCode);
        setAppliedDiscountCode(normalizedDiscountCode);
        setIsAppliedDiscountCodeNotValid(false);
    }, []);

    const syncFromResponse = useCallback(
        (jsonResponse) => syncFromDiscountCode(jsonResponse?.discountCode),
        [syncFromDiscountCode]);

    const reset = useCallback(() => syncFromDiscountCode(""), [syncFromDiscountCode]);

    const apply = useCallback((canApply) => {
        const normalizedDiscountCode = discountCode.trim();

        if (normalizedDiscountCode && canApply) {
            onSave(normalizedDiscountCode, true);
        }
    }, [discountCode, onSave]);

    const remove = useCallback(() => onSave(null, false), [onSave]);

    return {
        discountCode,
        appliedDiscountCode,
        isAppliedDiscountCodeNotValid,
        setDiscountCode,
        syncFromDiscountCode,
        syncFromResponse,
        apply,
        remove,
        reset
    };
};
