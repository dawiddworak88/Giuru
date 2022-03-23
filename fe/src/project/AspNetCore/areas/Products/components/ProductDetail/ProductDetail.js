import React, { Fragment, useContext, useEffect, useState } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { toast } from "react-toastify";
import { Button } from "@material-ui/core";
import ImageGallery from "react-image-gallery";
import Files from "../../../../shared/components/Files/Files";
import { Context } from "../../../../../../shared/stores/Store";
import Sidebar from "../../../../shared/components/Sidebar/Sidebar";
import CarouselGrid from "../../../../shared/components/CarouselGrid/CarouselGrid";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import Modal from "../../../../shared/components/Modal/Modal";

function ProductDetail(props) {
    const [state, dispatch] = useContext(Context);
    const [orderItems, setOrderItems] = useState(props.orderItems ? props.orderItems : []);
    const [basketId, setBasketId] = useState(props.basketId ? props.basketId : null);
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [totalQuantities, setTotalQuantities] = useState(0);

    const [product2, setProduct] = useState(null);

    const [t, st] = useState(true);

    const toggleSidebar = () => {
        setIsSidebarOpen(true)
    }

    const handleAddOrderItemClick = (item) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        let product = props;
        if (product2){
            product = {
                productId: product2.id, 
                sku: product2.subtitle, 
                title: product2.title,
                images: product2.images,
            }
        }

        const orderItem = {
            productId: product.productId,
            sku: product.sku,
            name: product.title,
            imageId: product.images ? product.images[0].id : null,
            quantity: item.quantity,
            stockQuantity: item.stockQuantity,
            outletQuantity: item.outletQuantity,
            externalReference: item.externalReference, 
            deliveryFrom: item.deliveryFrom, 
            deliveryTo: item.deliveryTo, 
            moreInfo: item.moreInfo
        }

        const basket = {
            id: basketId,
            items: [...orderItems, orderItem]
        };

        setTotalQuantities(orderItem.quantity + orderItem.stockQuantity + orderItem.outletQuantity);

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(basket)
        };

        fetch(props.updateBasketUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    dispatch({ type: "SET_TOTAL_BASKET", payload: parseInt(totalQuantities + state.totalBasketItems) })
                    
                    if (response.ok) {
                        setBasketId(jsonResponse.id);

                        if (jsonResponse.items && jsonResponse.items.length > 0) {
                            toast.success(props.successfullyAddedProduct)
                            setOrderItems(jsonResponse.items);
                            setIsModalOpen(false);
                        }
                        else {
                            setOrderItems([]);
                        }

                        setTotalQuantities(0);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const handleModal = (item) => {
        setIsModalOpen(true)
        setProduct(item);
        st(false);
    }

    useEffect(() => {
        if (isSidebarOpen){
            st(true);
        }

        if (!t && !isModalOpen){
            setIsSidebarOpen(true)
        }
    }, [t, isModalOpen, isSidebarOpen])

    return (
        <section className="product-detail section">
            <div className="product-detail__head columns is-tablet">
                <div className="column is-6">
                    {props.images && props.images.length &&
                        <div className="product-detail__image-gallery">
                            <ImageGallery items={props.images} />
                        </div>
                    }
                </div>
                <div className="column is-4">
                    <p className="product-detail__sku">{props.skuLabel} {props.sku}</p>
                    <h1 className="title is-4">{props.title}</h1>
                    <h2 className="product-detail__brand subtitle is-6">{props.byLabel} <a href={props.brandUrl}>{props.brandName}</a></h2>
                    {props.inStock && props.availableQuantity && props.availableQuantity > 0 &&
                        <div className="product-detail__in-stock">
                            {props.inStockLabel} {props.availableQuantity}
                            {props.expectedDelivery && 
                                <div className="product-detail__expected-delivery">{props.expectedDeliveryLabel} {moment.utc(props.expectedDelivery).local().format("L")}</div>
                            }
                        </div>
                    }
                    {props.inOutlet && props.availableOutletQuantity && props.availableOutletQuantity > 0 &&
                        <div className="product-detail__in-stock">
                            {props.inOutletLabel} {props.availableOutletQuantity}
                        </div>
                    }
                    {props.isAuthenticated && 
                        <div className="product-detail__add-to-cart-button">
                            {props.isProductVariant ? (
                                <div className="row">
                                    <Button type="text" variant="contained" color="primary" onClick={() => setIsModalOpen(true)}>
                                        {props.basketLabel}
                                    </Button>
                                </div>
                            ) : (
                                <div className="product-detail__add-to-cart-button">
                                    <Button type="text" variant="contained" color="primary" onClick={toggleSidebar}>
                                        {props.basketLabel}
                                    </Button>
                                </div>
                            )}
                        </div>
                    }
                    {props.description &&
                        <div className="product-detail__product-description">
                            <h3 className="product-detail__feature-title">{props.descriptionLabel}</h3>
                            <p>{props.description}</p>
                        </div>
                    }
                    {props.features &&
                        <div className="product-detail__product-information">
                            <h3 className="product-detail__feature-title">{props.productInformationLabel}</h3>
                            <div className="product-detail__product-information-list">
                                <dl>
                                    {props.features.map((item, index) =>
                                        <Fragment key={item.key}>
                                            <dt>{item.key}</dt>
                                            <dd>{item.value}</dd>
                                        </Fragment>
                                    )}
                                </dl>
                            </div>
                        </div>
                    }
                </div>
                <Sidebar 
                    productId={props.productId}
                    isOpen={isSidebarOpen}
                    manyUses={false}
                    setIsOpen={setIsSidebarOpen}
                    handleOrder={handleModal}
                    labels={props.sidebar}
                />
            </div>
            <CarouselGrid items={props.productVariants} />
            <Files {...props.files} />
            <Modal
                isOpen={isModalOpen}
                setIsOpen={setIsModalOpen}
                maxOutletValue={product2 ? product2.availableOutletQuantity : props.availableOutletQuantity}
                maxStockValue={product2 ? product2.availableQuantity : props.availableQuantity}
                handleOrder={handleAddOrderItemClick}
                labels={props.modal}
            />
        </section>
    );
}

ProductDetail.propTypes = {
    title: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    sku: PropTypes.string.isRequired,
    byLabel: PropTypes.string.isRequired,
    brandUrl: PropTypes.string.isRequired,
    brandName: PropTypes.string.isRequired,
    pricesLabel: PropTypes.string.isRequired,
    productInformationLabel: PropTypes.string.isRequired,
    inStockLabel: PropTypes.string,
    inStock: PropTypes.bool.isRequired,
    availableQuantity: PropTypes.number,
    restockableInDaysLabel: PropTypes.string,
    restockableInDays: PropTypes.number,
    descriptionLabel: PropTypes.string.isRequired,
    productDescription: PropTypes.string,
    productVariants: PropTypes.array,
    isProductVariant: PropTypes.bool,
    isAuthenticated: PropTypes.bool,
    images: PropTypes.array,
    files: PropTypes.object,
    sidebar: PropTypes.object,
    modal: PropTypes.object,
    addedProduct: PropTypes.string
};

export default ProductDetail;
