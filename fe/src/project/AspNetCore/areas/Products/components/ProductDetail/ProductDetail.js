import React, { Fragment } from 'react';
import PropTypes from 'prop-types';
import ImageGallery from 'react-image-gallery';
import {
    Fab, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, Button
} from '@material-ui/core';
import { GetApp, Link, LockOutlined } from '@material-ui/icons';
import moment from 'moment';

function ProductDetail(props) {

    const handleDownloadClick = (file) => {
    }

    const handleCopyClick = (file) => {
    }

    return (

        <section className="product-detail section">
            <div className="columns is-tablet">
                <div className="column is-6">
                    <div className="product-detail__image-gallery">
                        <ImageGallery items={props.images} />
                    </div>
                </div>
                <div className="column is-4">
                    <p className="product-detail__sku">{props.skuLabel} {props.sku}</p>
                    <h1 className="title is-4">{props.title}</h1>
                    <h2 className="product-detail__brand subtitle is-6">{props.byLabel} <a href={props.brandUrl}>{props.brandName}</a></h2>
                    {props.inStock &&
                        <div className="product-detail__in-stock">
                            {props.inStockLabel}
                        </div>
                    }
                    <div className="product-detail__price">
                        <h3 className="product-detail__feature-title">{props.pricesLabel}</h3>
                        {!props.isAuthenticated &&
                            <a href={props.signInUrl}>
                                {props.signInToSeePricesLabel}
                            </a>
                        }
                    </div>
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
                                    <Fragment key={index}>
                                        <dt>{item.key}</dt>
                                        <dd>{item.value}</dd>
                                    </Fragment>
                                )}
                                </dl>
                            </div>
                        </div>
                    }
                </div>
            </div>
            {props.files &&
                <div className="product-detail__download">
                    <h3 className="product-detail__download-title">{props.downloadLabel}</h3>
                    <div className="table-container">
                        <div className="catalog__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.downloadLabel}>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell width="11%"></TableCell>
                                            <TableCell>{props.filenameLabel}</TableCell>
                                            <TableCell>{props.nameLabel}</TableCell>
                                            <TableCell>{props.descriptionLabel}</TableCell>
                                            <TableCell>{props.sizeLabel}</TableCell>
                                            <TableCell>{props.lastModifiedDateLabel}</TableCell>
                                            <TableCell>{props.createdDateLabel}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {props.files.map((file) => (
                                            <TableRow key={file.id}>
                                                <TableCell width="11%">
                                                    <Fab onClick={() => handleDownloadClick(file)} size="small" color="primary" aria-label={props.deleteLabel}>
                                                        <GetApp />
                                                    </Fab>
                                                    <Fab onClick={() => handleCopyClick(file)} size="small" color="secondary" aria-label={props.copyLinkLabel}>
                                                        <Link />
                                                    </Fab>
                                                </TableCell>
                                                <TableCell>
                                                    <Button variant="text" onClick={() => handleDownloadClick(file)}>
                                                        {file.filename}
                                                        {(file.isProtected && !props.isAuthenticated) &&
                                                            <LockOutlined color="primary" />
                                                        }
                                                    </Button>
                                                </TableCell>
                                                <TableCell>{file.name}</TableCell>
                                                <TableCell>{file.description}</TableCell>
                                                <TableCell>{file.size}</TableCell>
                                                <TableCell>{moment(file.lastModifiedDate).local().format('L LT')}</TableCell>
                                                <TableCell>{moment(file.createdDate).local().format('L LT')}</TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </div>
                    </div>
                </div>
            }
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
    inStockLabel: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string.isRequired,
    downloadLabel: PropTypes.string.isRequired,
    filenameLabel: PropTypes.string.isRequired,
    sizeLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    lastModifiedDateLabel: PropTypes.string.isRequired,
    createdDateLabel: PropTypes.string.isRequired,
    productDescription: PropTypes.string
}

export default ProductDetail;