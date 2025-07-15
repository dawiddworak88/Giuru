import React, { Fragment, useContext, useEffect, useState } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { toast } from "react-toastify";
import { Button } from "@mui/material";
import Files from "../../../../shared/components/Files/Files";
import { Context } from "../../../../shared/stores/Store";
import Sidebar from "../../../../shared/components/Sidebar/Sidebar";
import CarouselGrid from "../../../../shared/components/CarouselGrid/CarouselGrid";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import Modal from "../../../../shared/components/Modal/Modal";
import { ExpandMore, ExpandLess } from "@mui/icons-material"
import { marked } from "marked";
import ResponsiveImage from "../../../../shared/components/Picture/ResponsiveImage";
import { Splide, SplideSlide } from "@splidejs/react-splide";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../shared/constants/LazyLoadConstants";
import ProductDetailModal from "../ProductDetailModal/ProductDetailModal";
import Price from "../../../../shared/components/Price/Price";

function ProductDetail(props) {
    const [state, dispatch] = useContext(Context);
    const [orderItems, setOrderItems] = useState(props.orderItems ? props.orderItems : []);
    const [basketId, setBasketId] = useState(props.basketId ? props.basketId : null);
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isImageModalOpen, setIsImageModalOpen] = useState(false);
    const [productVariant, setProductVariant] = useState(null);
    const [canActiveModal, setCanActiveModal] = useState(true);
    const [showMore, setShowMore] = useState(false);
    const [showMoreImages, setShowMoreImages] = useState(false);
    const [mediaItems, setMediaItems] = useState(props.mediaItems ? props.mediaItems.slice(0, 6) : []);
    const [activeMediaItemIndex, setActiveMediaItemIndex] = useState(0);

    const handleAddOrderItemClick = (item) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        let product = props;
        if (productVariant) {
            product = {
                productId: productVariant.id,
                sku: productVariant.subtitle,
                title: productVariant.title,
                images: productVariant.images,
                price: productVariant.price
            }
        }

        const quantity = parseInt(item.quantity);
        const stockQuantity = parseInt(item.stockQuantity);
        const outletQuantity = parseInt(item.outletQuantity);

        const totalQuantity = quantity + stockQuantity + outletQuantity;

        if (props.maxAllowedOrderQuantity && 
           (totalQuantity > props.maxAllowedOrderQuantity)) {
                toast.error(props.maxAllowedOrderQuantityErrorMessage);
                dispatch({ type: "SET_IS_LOADING", payload: false });
                return;
        };

        const orderItem = {
            productId: product.productId,
            sku: product.sku,
            name: product.title,
            imageId: product.images ? product.images[0].id : null,
            quantity: quantity,
            stockQuantity: stockQuantity,
            outletQuantity: outletQuantity,
            externalReference: item.externalReference,
            unitPrice: product.price ? parseFloat(product.price.current).toFixed(2) : null,
            price: product.price ? parseFloat(product.price.current * totalQuantity).toFixed(2) : null,
            currency: product.price ? product.price.currency : null,
            deliveryFrom: moment(item.deliveryFrom).startOf("day"),
            deliveryTo: moment(item.deliveryTo).startOf("day"),
            moreInfo: item.moreInfo
        }

        const basket = {
            id: basketId,
            items: [...orderItems, orderItem]
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(basket)
        };

        if (totalQuantity <= 0) {
            return toast.error(props.quantityErrorMessage);
        }

        fetch(props.updateBasketUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    dispatch({ type: "SET_TOTAL_BASKET", payload: parseInt(totalQuantity + state.totalBasketItems) })

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

    const handleCloseImageModal = () => {
        setIsImageModalOpen(false);
    }

    const handleImageModal = (index) => {
        setActiveMediaItemIndex(index);
        setIsImageModalOpen(true);
    }

    const handleCloseModal = () => {
        setIsModalOpen(false);
    }

    const handleModal = (item) => {
        setIsModalOpen(true);
        setProductVariant(item);
        setCanActiveModal(false);
    }

    useEffect(() => {
        if (isSidebarOpen) {
            setCanActiveModal(true);
        }

        if (!canActiveModal && !isModalOpen) {
            setIsSidebarOpen(true)
        }
    }, [canActiveModal, isModalOpen, isSidebarOpen]);

    const handleShowMoreImages = () => {
        if (showMoreImages) {
            setMediaItems(props.mediaItems.slice(0, 6));
        }
        else {
            setMediaItems(props.mediaItems);
        }

        setShowMoreImages(!showMoreImages);
    }

    return (
        <section className="product-detail section">
            <div className="product-detail__container">
                <div className="product-detail__head columns is-desktop">
                    <div className="product-detail__gallery-column">
                        <div className="product-detail__desktop-gallery">
                            <div className="is-flex is-flex-wrap product-detail__product-gallery">
                                {props.mediaItems && props.mediaItems.length > 1 ? (
                                    mediaItems.map((mediaItem, index) => (
                                        <div className="product-detail__desktop-gallery__desktop-product-image" onClick={() => handleImageModal(index)}>
                                            <LazyLoad offset={LazyLoadConstants.defaultOffset()} key={index}>
                                                {mediaItem.mimeType.startsWith("image") ? (
                                                    <ResponsiveImage sources={mediaItem.sources} imageSrc={mediaItem.mediaSrc} imageAlt={mediaItem.mediaAlt} imageTitle={props.title} />
                                                ) : (
                                                    <video autoPlay loop muted playsInline preload='auto'>
                                                        <source src={mediaItem.mediaSrc} type={mediaItem.mimeType} />
                                                    </video>
                                                )}
                                            </LazyLoad>
                                        </div>
                                    ))
                                ) : (
                                    props.mediaItems.length == 1 && (
                                        <div className="product-detail__desktop-gallery__desktop-product-image__single" onClick={() => handleImageModal(0)}>
                                            <LazyLoad offset={LazyLoadConstants.defaultOffset()}>
                                                {props.mediaItems[0].mimeType.startsWith("image") ? (
                                                    <ResponsiveImage sources={props.mediaItems[0].sources} imageSrc={props.mediaItems[0].mediaSrc} imageAlt={props.mediaItems[0].mediaAlt} imageTitle={props.title} />
                                                ) : (
                                                    <video autoPlay loop muted playsInline preload='auto'>
                                                        <source src={props.mediaItems[0].mediaSrc} type={props.mediaItems[0].mimeType} />
                                                    </video>
                                                )}
                                            </LazyLoad>
                                        </div>
                                    )
                                )}

                            </div>
                            {props.mediaItems && props.mediaItems.length > 6 &&
                                <div className="product-detail__more-product-images">
                                    {showMoreImages ? (
                                        <div className="is-flex is-justify-content-center">
                                            <Button
                                                variant="outlined"
                                                onClick={handleShowMoreImages}
                                                color="inherit">
                                                {props.seeLessText}
                                            </Button>
                                        </div>
                                    ) : (
                                        <div className="is-flex is-justify-content-center">
                                            <Button
                                                variant="outlined"
                                                onClick={handleShowMoreImages}
                                                color="inherit">
                                                {props.seeMoreText}
                                            </Button>
                                        </div>
                                    )}
                                </div>
                            }
                        </div>
                        <div className="product-detail__mobile-gallery">
                            {props.mediaItems && props.mediaItems.length > 0 &&
                                <Splide
                                    options={{
                                        type: "slide",
                                        pagination: false
                                    }}
                                >
                                    {props.mediaItems.map((mediaItem, index) => {
                                        return (
                                            <SplideSlide key={index}>
                                                <LazyLoad offset={LazyLoadConstants.defaultOffset()} className="product-detail__mobile-gallery__mobile-product-image">
                                                    {mediaItem.mimeType.startsWith("image") ? (
                                                        <ResponsiveImage sources={mediaItem.sources} imageSrc={mediaItem.mediaSrc} imageAlt={mediaItem.mediaAlt} imageTitle={props.title} />
                                                    ) : (
                                                        <video autoPlay loop muted playsInline preload='auto'>
                                                            <source src={mediaItem.mediaSrc} type={mediaItem.mimeType} />
                                                        </video>
                                                    )}
                                                </LazyLoad>
                                            </SplideSlide>
                                        )
                                    })
                                    }
                                </Splide>
                            }
                        </div>
                    </div>
                    <div className="product-detail__description-column">
                        <p className="product-detail__sku">{props.skuLabel} {props.sku}</p>
                        {props.ean &&
                            <p className="product-detail__ean">{props.eanLabel} {props.ean}</p>
                        }
                        <h1 className="title is-4 mt-1">{props.title}</h1>
                        <h2 className="product-detail__brand subtitle is-6">{props.byLabel} <a href={props.brandUrl}>{props.brandName}</a></h2>
                        {props.outletTitle &&
                            <div className="product-details__discount">{props.outletTitleLabel} {props.outletTitle}</div>
                        }
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
                        {props.price &&
                            <Price 
                                {...props.price}
                            />
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
                                        <Button type="text" variant="contained" color="primary" onClick={() => setIsSidebarOpen(true)}>
                                            {props.basketLabel}
                                        </Button>
                                    </div>
                                )}
                            </div>
                        }
                        {props.description &&
                            <div className="product-detail__product-description">
                                <h3 className="product-detail__feature-title">{props.descriptionLabel}</h3>
                                <div dangerouslySetInnerHTML={{ __html: marked.parse(props.description) }}></div>
                            </div>
                        }
                        {props.features && props.features.length > 0 &&
                            <div className="mt-2">
                                {showMore ? (
                                    <Fragment>
                                        <div className="is-flex is-justify-content-center">
                                            <span className="is-flex is-align-content-center is-text button" onClick={() => setShowMore(false)}>{props.readLessText} <ExpandLess /></span>
                                        </div>
                                        <div className="product-detail__product-information">
                                            <h3 className="product-detail__feature-title">{props.productInformationLabel}</h3>
                                            <div className="product-detail__product-information-list">
                                                <dl>
                                                    {props.features.map((item, index) =>
                                                        item.value && item.value.trim() !== '' && (
                                                            <Fragment key={item.key}>
                                                                <dt>{item.key}</dt>
                                                                <dd>{item.value}</dd>
                                                            </Fragment>
                                                    ))}
                                                </dl>
                                            </div>
                                        </div>
                                    </Fragment>
                                ) : (
                                    <div className="is-flex is-justify-content-center">
                                        <span className="button is-flex is-align-content-center is-text" onClick={() => setShowMore(true)}>{props.readMoreText} <ExpandMore /></span>
                                    </div>
                                )}
                            </div>
                        }
                    </div>
                </div>
                <Sidebar
                    productId={props.productId}
                    isOpen={isSidebarOpen}
                    manyUses={false}
                    setIsOpen={setIsSidebarOpen}
                    handleOrder={handleModal}
                    labels={props.sidebar}
                />
                <CarouselGrid items={props.productVariants} className="pt-6" />
                {props.files &&
                    <Files {...props.files} />
                }
                <Modal
                    isOpen={isModalOpen}
                    setIsOpen={setIsModalOpen}
                    handleClose={handleCloseModal}
                    maxOutletValue={productVariant ? productVariant.availableOutletQuantity : props.availableOutletQuantity}
                    maxStockValue={productVariant ? productVariant.availableQuantity : props.availableQuantity}
                    handleOrder={handleAddOrderItemClick}
                    product={productVariant ? productVariant : props}
                    labels={props.modal}
                />
                <ProductDetailModal
                    isOpen={isImageModalOpen}
                    handleClose={handleCloseImageModal}
                    mediaItems={props.mediaItems}
                    title={props.title}
                    index={activeMediaItemIndex}
                />
            </div>
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
    addedProduct: PropTypes.string,
    eanLabel: PropTypes.string.isRequired,
    readLessText: PropTypes.string.isRequired,
    readMoreText: PropTypes.string.isRequired,
    maxAllowedOrderQuantity: PropTypes.number,
    maxAllowedOrderQuantityErrorMessage: PropTypes.string
};

export default ProductDetail;
